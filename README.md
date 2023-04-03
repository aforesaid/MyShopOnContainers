# MyShopOnContainers

Service interaction scheme

![Scheme](https://github.com/bezlla/MyShopOnContainers/blob/dev/Scheme.jpg)

## Stack
* .NET 6.0, [ASP .NET 6.0](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
* MassTransit 8.0 (RabbitMQ) [Saga StateMachine](https://masstransit.io/documentation/patterns/saga/state-machine), [CourierActivities](https://masstransit.io/documentation/patterns/routing-slip)
* [EntityFramework Core 7.0 (PostgreSQL)](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [Docker](https://www.docker.com/)

## Build

```
docker-compose build
```

## Deploy

```
docker-compose up
```
