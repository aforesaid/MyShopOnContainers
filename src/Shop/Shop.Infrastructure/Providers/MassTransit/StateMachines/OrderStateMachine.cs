using Interfaces.Shop.Events;
using Interfaces.Stock.Events;
using Interfaces.Stock.Messages;
using MassTransit;
using MediatR;
using Shop.Domain.Enums;
using Shop.Infrastructure.Providers.MassTransit.StateMachines.OrderStateMachineActivities;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines;

public class OrderStateMachine  
    : MassTransitStateMachine<OrderState>

{
    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockOutOfStock,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReleased,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReservationCancelled,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderFaulted,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderCancellationStarted,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderReleaseStockStarted,
            x => x.CorrelateById(m => m.Message.OrderId));
        
        Initially(When(OrderCreated)
                .TransitionTo(Created)
                .Activity(x => x.OfType<AcceptOrderActivity>()));

        During(Created,
            When(StockReserved)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Reserved);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Reserved),
            When(StockOutOfStock)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Rejected);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Faulted));
            When(OrderFaulted)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Rejected);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Faulted);

            During(Reserved,
                When(OrderFaulted)
                    .ThenAsync(async context =>
                    {
                        var mediator = context.GetServiceOrCreateInstance<IMediator>();
                        var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                            OrderStatesEnum.Rejected);
                        await mediator.Send(setOrderStateRequest);
                    })
                    .TransitionTo(Faulted),
                When(OrderCancellationStarted)
                    .TransitionTo(CancelProcessing)
                    .Activity(x => x.OfType<CancelOrderActivity>()),
                When(OrderReleaseStockStarted)
                    .TransitionTo(ReleaseProcessing)
                    .Activity(x => x.OfType<ReleaseOrderActivity>()));

            During(ReleaseProcessing,
            When(StockReleased)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Completed);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Completed));
       
        During(CancelProcessing,
            When(StockReservationCancelled)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Canceled);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Canceled));
    }
    
    public State Created { get; set; }
    public State Reserved { get; set; }
    public State ReleaseProcessing { get; set; }
    public State CancelProcessing { get; set; }
    public State Completed { get; set; }
    public State Canceled { get; set; }
    public State Faulted { get; set; }
    
    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<StockReserved> StockReserved { get; set; }
    public Event<StockOutOfStock> StockOutOfStock { get; set; }
    public Event<StockReservationCancelled> StockReservationCancelled { get; set; }
    public Event<StockReleased> StockReleased { get; set; }
    public Event<OrderFaulted> OrderFaulted { get; set; }
    public Event<OrderReleaseStockStarted> OrderReleaseStockStarted { get; set; }
    public Event<OrderCancellationStarted> OrderCancellationStarted { get; set; }
}