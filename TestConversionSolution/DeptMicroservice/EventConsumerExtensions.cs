using Autofac;
using DeptMicroservice.Consumers;
using DiDrDe.MessageBus.Infra.MassTransit.Autofac.Factories;
using DiDrDe.MessageBus.Infra.MassTransit.BusManagers;
using DiDrDe.MessageBus.Infra.MassTransit.Configuration;
using DiDrDe.MessageBus.Infra.MassTransit.Contracts.BusControls;
using DiDrDe.MessageBus.Infra.MassTransit.Contracts.BusManagers;
using System;

namespace DiDrDe.MessageBus.Infra.MassTransit.Autofac
{
    public static class EventConsumerExtensions
    {
        public static ContainerBuilder RegisterActiveMqEventConsumer(
            this ContainerBuilder builder,
            Action<EventTypesOptions> eventOptions,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever)
        {
            var eventsOptions = new EventTypesOptions();
            eventOptions?.Invoke(eventsOptions);
            var eventsTypes = eventsOptions.EventTypes;

            foreach (var eventType in eventsTypes)
            {
                var adapterType = typeof(EventConsumerAdapter<>).MakeGenericType(eventType);

                builder
                    .RegisterType(adapterType)
                    .SingleInstance();
            }

            builder
                .Register(context =>
                {
                    var busControlWrapper =
                        ActiveMqBusControlWrapperFactory.CreateForEventConsumer(context, optionsRetriever, eventsTypes);
                    return busControlWrapper;
                })
                .As<IEventConsumerMessageBusControl>()
                .SingleInstance();

            builder
                .RegisterType<MessageBusManager<IEventConsumerMessageBusControl>>()
                .As<IEventConsumerMessageBusManager>()
                .SingleInstance();

            return builder;
        }
    }
}