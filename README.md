# MyShopOnContainers

Схема взаимодействия сервисов

![Scheme](https://github.com/bezlla/MyShopOnContainers/blob/master/images/Scheme.jpg)

## Stack
* .NET 6.0, [ASP .NET 6.0](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
* MassTransit 8.0 (RabbitMQ) [Saga StateMachine](https://masstransit.io/documentation/patterns/saga/state-machine), [CourierActivities](https://masstransit.io/documentation/patterns/routing-slip)
* [EntityFramework Core 7.0 (PostgreSQL)](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [MediatR 12.0](https://github.com/jbogard/MediatR)
* [Serilog](https://github.com/serilog/serilog)
* [Docker](https://www.docker.com/)

## Patterns
* Choreography
* Saga
* Database per service
* DDD

## Содержание
1. [Описание](#Описание)
2. Сервисы
   1. [Web API](#Web-API)
   2. [Shop](#Shop)
   3. [Stock](#Stock)
3. [Docker](#Docker)

### Описание

Целью проекта является демонстрация паттерна Choreography с использованием MassTransit (RabbitMQ), Saga и Courier Activities.

В его основе лежит 2 stateless-микросервиса Shop & Stock, коммуникация между которыми практически полностью сведена к событийной модели.
Web API служит шлюзом для пользовательского взаимодействия.

#### Web API

> http://localhost:5000/swagger

![Swagger](https://github.com/bezlla/MyShopOnContainers/blob/master/images/Swagger.jpg)

Типовый алгоритм использования:

1. Просмотр доступных продуктов (Stock/GetProducts)
2. Создание заказа (Shop/CreateOrder)
3. Просмотр заказа (Shop/GetOrders)
4. Действие с заказом, если есть возможность (Shop/ReleaseOrder, Shop/CancelOrder)
5. Просмотр заказа (Shop/GetOrders)

##### Конфигурация

Файл конфигурации - ```appsettings.json```.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "restrictedToMinimumLevel": "Debug"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "AllowedHosts": "*",
  
  "RABBIT_HOST": "localhost",
  "RABBIT_LOGIN": "guest",
  "RABBIT_PASSWORD": "guest"
}
```

Данный сервис представляет из себя ASP .NET Web API приложение, в котором имеется 2 контроллера.

##### ShopController

1. Просмотреть список заказов для пользователя. (GetOrders) ```GET```

В параметрах запроса необходимо указать ```userId```, который указывали при создании заказа.

```json
{
  "orders": [
    {
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "orderId": "9e7a3842-3a74-43ab-af1e-a66903c0b17d",
      "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 10,
      "orderState": 10,
      "created": "2023-04-03T19:28:20.221949Z",
      "updated": "2023-04-03T19:28:20.221949Z"
    }
  ]
}
```

Данный метод возвращает полную информацию по заказам.

Статусы заказов:

```
Created - 10, (создан)
Reserved - 20, (забронирован)
Completed - 40, (завершен)
Canceled - 90, (отменен)
Faulted - 100 (ошибка заказа)
```

2. Создать заказ. (CreateOrder) ```POST```

Тело запроса выглядит следующим образом:
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 10
}
```

```userId``` - Id пользователя, учитывая, что нет единой системы пользователей подойдет абсолютно любой Guid

```productId``` - Id продукта, взять его можно из метода ```Stock/GetProducts```

```quantity``` - количество товаров. Значение должно быть больше 0.

В ответе при успешном создании заказа возвращается номер заказа, иначе информация об ошибке:
```json
{
  "orderId": "9e7a3842-3a74-43ab-af1e-a66903c0b17d",
  "success": true,
  "errorReason": null
}
```

3. Завершение заказа или Отмена заказа. (ReleaseOrder & CancelOrder) ```PUT```

Действие возможно только для тех заказов, статус которых ```Reserved```.
При выполнении запроса необходимо указать ```orderId```.

##### StockController

1. Просмотр доступных продуктов. (GetProducts) ```GET```

```json
{
  "products": [
    {
      "productId": "5a14b100-6e39-4c7a-b73b-418c600e9f96",
      "productName": "Product 1",
      "free": 100
    }
  ]
}
```

Возвращает список товаров и информацию по ним:

```productName``` - название товаров

```free``` - количество доступных товаров

2. Создать новый товар. (AddProduct) ```POST```

При создании товара указываются следующий параметры:
```json
{
  "productName": "Product 1",
  "available": 100
}
```
где ```available``` количество доступных товаров.


В ответе возвращается Id товара:
```json
{
  "productId": "5a14b100-6e39-4c7a-b73b-418c600e9f96"
}
```

3. Создание поставки товаров. (CreateSupply) ```POST```

При создании поставки указывается Id товара и его количество:
```json
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 100
}
```

#### Shop

Данный сервис является ключевым в данной системе, представляет из себя консольное .NET 6.0 приложение.

##### Конфигурация

Конфигурация сервиса выглядит следующим образом:
```json
{
  "ConnectionStrings": {
    "POSTGRES": "User ID=postgres;Password=password;Server=localhost;Port=5432;Database=shop;Integrated Security=true;",
    "MongoDb": "mongodb://root:myPassword1@localhost:27017"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "restrictedToMinimumLevel": "Debug"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "RABBIT_HOST": "localhost",
  "RABBIT_LOGIN": "guest",
  "RABBIT_PASSWORD": "guest",
  "PrefetchCount": 100
}
```

В секции ```ConnectionStrings``` задаются параметры для подключения к PostgreSQL и Mongo.

##### Consumers

> [ShopGetOrdersConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/Consumers/ShopGetOrdersConsumer.cs) ```Request/Response``` - получение список заказов 

> [ShopCreateOrderConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/Consumers/ShopCreateOrderConsumer.cs) ```Request/Response``` - создание заказа

> [ShopReleaseProductForOrderConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/Consumers/ShopReleaseProductForOrderConsumer.cs) - завершение заказа

> [ShopCancelOrderConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/Consumers/ShopCancelOrderConsumer.cs) - отмена заказа

> [ShopReserveProductForOrderConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/Consumers/ShopReserveProductForOrderConsumer.cs) - резервирование товаров для заказа 

В последнем используется реализация ```RoutingSlip``` от MassTransit с использованием RabbitMQ.

В данном случае маршрут состоит из двух Activity: ```OrderStockStatusActivity``` и ```OrderReserveProductActivity```

```C#
public async Task Consume(ConsumeContext<ShopReserveProductForOrderCommand> context)
{
   var request = context.Message;

   var builder = new RoutingSlipBuilder(NewId.NextGuid());

   builder.AddActivity(nameof(OrderStockStatusActivity), 
   _provider.GetExecuteEndpoint<OrderStockStatusActivity, OrderStockStatusActivityArguments>(),
      new OrderStockStatusActivityArguments(orderId: request.OrderId));

   builder.AddActivity(nameof(OrderReserveProductActivity), 
   _provider.GetExecuteEndpoint<OrderReserveProductActivity, OrderReserveProductActivityArguments>(),
      new OrderReserveProductActivityArguments(orderId: request.OrderId));

   await builder.AddSubscription(context.SourceAddress,
      RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
      RoutingSlipEventContents.None,
      x => x.Send(new OrderFaulted(request.OrderId)));

   var routingSlip = builder.Build();

   await context.Execute(routingSlip);
}
```

##### Activities

> [OrderStockStatusActivity](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/CouriersActivities/OrderStockStatusActivity.cs) - валидация товаров, указанных в заказе, проверка наличия их на складе

> [OrderReserveProductActivity](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/CouriersActivities/OrderReserveProductActivity.cs) - запуск резервирования товаров на складе для заказа

##### StateMachines

> [OrderStateMachine](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/StateMachines/OrderStateMachine.cs)

###### Конфигурация стейт-машины

```C#
serviceCollection.AddMassTransit(x =>
{
   ...
   x.UsingRabbitMq((context, cfg) =>
   {
      ...
      x.AddSagaStateMachine<OrderStateMachine, OrderState>()
         .MongoDbRepository(r =>
      {
        r.Connection = configuration.GetConnectionString("MongoDb");
        r.DatabaseName = ordersDatabase;
      });
      ...
   }
}
```
StateMachine состоит из 7 статусов, каждый из которых имеет альтернативу с доменным статусом заказов, кроме двух: ```ReleaseProcessing``` и ```CancelProcessing``` - данные статусы промежуточные.

В ```OrderStateMachine``` кроме внутренних событий по заказам также используются внешние по складу от сервиса ```Stock```, при этом сервисы в данном контексте ничего друг о друге не знают, так как события публикуются для всех подписчиков.

###### Activities

Данные активности являются частью бизнес-логики ```OrderStateMachine```.

> [AcceptOrderActivity](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/StateMachines/OrderStateMachineActivities/AcceptOrderActivity.cs) - инициация резервирования заказа

> [CancelOrderActivity](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/StateMachines/OrderStateMachineActivities/CancelOrderActivity.cs) - инициация отмены заказа

> [ReleaseOrderActivity](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Shop/Shop.Infrastructure/Providers/MassTransit/StateMachines/OrderStateMachineActivities/ReleaseOrderActivity.cs) - инициация завершения заказа

#### Stock

Конфигурация сервиса схожа с сервисом ```Shop```, отдельного рассмотрения не требует.

##### Consumers

> [StockGetProductListConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockGetProductListConsumer.cs) ```Request/Response``` - получение списка товаров

> [StockAddProductConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockAddProductConsumer.cs) ```Request/Response``` - добавление нового товара

> [StockSupplyProductConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockSupplyProductConsumer.cs) ```Request/Response``` - создание новой поставки, добавление доступных товаров для существующего продукта

> [StockCancelReservationProductConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockCancelReservationProductConsumer.cs) - отмена брони товара

> [StockReleaseProductConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockReleaseProductConsumer.cs) - выпуск товара

> [StockReserveProductConsumer](https://github.com/bezlla/MyShopOnContainers/blob/master/src/Stock/Stock.Infrastructure/Providers/MassTransit/Consumers/StockReserveProductConsumer.cs) - бронирование товара

### Docker

Данный проект поддерживает docker.

Для его запуска достаточно прописать:

```
docker-compose build
docker-compose up
```

Детали конфигурации docker-compose вы можете найти [здесь](https://github.com/bezlla/MyShopOnContainers/blob/master/docker-compose.yml).
