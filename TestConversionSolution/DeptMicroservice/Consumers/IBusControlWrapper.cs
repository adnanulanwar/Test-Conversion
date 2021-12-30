

using DiDrDe.MessageBus.Infra.MassTransit.Contracts.BusControls;

namespace DiDrDe.MessageBus.Infra.MassTransit.Autofac.Contracts
{
    public interface IBusControlWrapper
        :  IEventConsumerMessageBusControl
    {
    }
}