# MyShopOnContainers

Схема взаимодействия сервисов

![Scheme](https://github.com/bezlla/MyShopOnContainers/blob/dev/images/Scheme.jpg)

## Stack
* .NET 6.0, [ASP .NET 6.0](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
* MassTransit 8.0 (RabbitMQ) [Saga StateMachine](https://masstransit.io/documentation/patterns/saga/state-machine), [CourierActivities](https://masstransit.io/documentation/patterns/routing-slip)
* [EntityFramework Core 7.0 (PostgreSQL)](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [MediatR 12.0](https://github.com/jbogard/MediatR)
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

![Swagger](https://github.com/bezlla/MyShopOnContainers/blob/dev/images/Swagger.jpg)

Типовый алгоритм использования:

1. Просмотр доступных продуктов (Stock/GetProducts)
2. Создание заказа (Shop/CreateOrder)
3. Просмотр заказа (Shop/GetOrders)
4. Действие с заказом, если есть возможность (Shop/ReleaseOrder, Shop/CancelOrder)
5. Просмотр заказа (Shop/GetOrders)

Данный сервис представляет из себя ASP .NET Web API приложение, в котором имеется 2 контроллера.

##### ShopController

Функционал: 

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

#### Stock

### Docker

Данный проект поддерживает docker.

Для его запуска достаточно прописать:

```
docker-compose build
docker-compose up
```

Детали конфигурации docker-compose вы можете найти [здесь](https://github.com/bezlla/MyShopOnContainers/blob/dev/docker-compose.yml).
