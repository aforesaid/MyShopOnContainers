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




#### Shop
#### Stock

### Docker

```
docker-compose build
docker-compose up
```
