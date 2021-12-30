using Autofac;
using DiDrDe.MessageBus.Infra.MassTransit.Autofac.BusControls;
using DiDrDe.MessageBus.Infra.MassTransit.Autofac.Contracts;
using DiDrDe.MessageBus.Infra.MassTransit.Configuration;
using GreenPipes;
using MassTransit;
using MassTransit.ActiveMqTransport;
using System;
using System.Collections.Generic;

namespace DiDrDe.MessageBus.Infra.MassTransit.Autofac.Factories
{
    public static class ActiveMqBusControlWrapperFactory
    {
        public static IBusControlWrapper CreateForCommandConsumer(
            IComponentContext context,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever,
            IReadOnlyDictionary<Type, Type> commandsTypes)
        {
            var ctx = context.Resolve<IComponentContext>();
            var messageBusOptions = optionsRetriever.Invoke(ctx);
            var queueName = messageBusOptions.EndpointName;

            var busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
            {
                var activeMqHost = CreateActiveMqHost(cfg, messageBusOptions);

                cfg.ReceiveEndpoint(activeMqHost, queueName, consumer =>
                {
                    foreach (var types in commandsTypes)
                    {
                        var typeArguments =
                            new[]
                            {
                                types.Key,
                                types.Value
                            };
                        var adapterTypeWithTypeArguments = typeof(CommandConsumerAdapter<,>).MakeGenericType(typeArguments);
                        var resolvedType = ctx.Resolve(adapterTypeWithTypeArguments);

                        consumer.Consumer(adapterTypeWithTypeArguments, type => resolvedType);
                    }
                });
            });

            var busControlWrapper = new BusControlWrapper(busControl);
            return busControlWrapper;
        }

        public static IBusControlWrapper CreateForCommandSender(
            IComponentContext context,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever,
            IReadOnlyDictionary<Type, Type> commandTypes)
        {
            var ctx = context.Resolve<IComponentContext>();
            var messageBusOptions = optionsRetriever.Invoke(ctx);
            var queueName = messageBusOptions.EndpointName;

            var busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
            {
                CreateActiveMqHost(cfg, messageBusOptions);
            });

            var uriType = typeof(Uri);
            var parametersTypes =
                new[]
                {
                    uriType.MakeByRefType()
                };

            var serverAddress = busControl.Address;
            var uri = new Uri(serverAddress, queueName);

            var endPointConventionTryGetDestinationAddress = typeof(EndpointConvention).GetMethod("TryGetDestinationAddress", parametersTypes);
            foreach (var commandType in commandTypes.Keys)
            {
                var endPointConventionTryGetDestinationAddressGeneric = endPointConventionTryGetDestinationAddress?.MakeGenericMethod(commandType);
                var tryGetDestinationAddressParameters =
                    new object[]
                    {
                        uri
                    };

                endPointConventionTryGetDestinationAddressGeneric?.Invoke(null, tryGetDestinationAddressParameters);
            }

            var busControlWrapper = new BusControlWrapper(busControl);
            return busControlWrapper;
        }

        public static IBusControlWrapper CreateForEventConsumer(
            IComponentContext context,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever,
            IReadOnlyList<Type> eventsTypes)
        {
            var ctx = context.Resolve<IComponentContext>();
            var messageBusOptions = optionsRetriever.Invoke(ctx);
            var queueName = messageBusOptions.EndpointName;
            var retryPolicyOptions = messageBusOptions.RetryPolicyOptions;
            var busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
            {
                var activeMqHost = CreateActiveMqHost(cfg, messageBusOptions);

                cfg.ReceiveEndpoint(activeMqHost, queueName, consumer =>
                {
                    ConfigureRetryPolicy(consumer, retryPolicyOptions);

                    foreach (var eventDtoType in eventsTypes)
                    {
                        var adapterType = typeof(EventConsumerAdapter<>).MakeGenericType(eventDtoType);
                        var resolvedType = ctx.Resolve(adapterType);

                        consumer.Consumer(adapterType, type => resolvedType);
                    }
                });
            });

            var busControlWrapper = new BusControlWrapper(busControl);
            return busControlWrapper;
        }

        public static IBusControlWrapper CreateForEventPublisher(
            IComponentContext context,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever)
        {
            var ctx = context.Resolve<IComponentContext>();
            var messageBusOptions = optionsRetriever.Invoke(ctx);
            var busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
            {
                CreateActiveMqHost(cfg, messageBusOptions);
            });

            var busControlWrapper = new BusControlWrapper(busControl);
            return busControlWrapper;
        }

        public static IBusControlWrapper CreateForHealthChecker(
            IComponentContext context,
            Func<IComponentContext, ActiveMqOptions> optionsRetriever)
        {
            var ctx = context.Resolve<IComponentContext>();
            var messageBusOptions = optionsRetriever.Invoke(ctx);
            var busControl = Bus.Factory.CreateUsingActiveMq(cfg =>
            {
                CreateActiveMqHost(cfg, messageBusOptions);
                cfg.Durable = false;
                cfg.Lazy = true;
            });

            var busControlWrapper = new BusControlWrapper(busControl);
            return busControlWrapper;
        }

        private static IActiveMqHost CreateActiveMqHost(IActiveMqBusFactoryConfigurator configurator, ActiveMqOptions options)
        {
            var hostName = options.HostName;
            var port = options.Port;
            var username = options.Username;
            var password = options.Password;
            var useSsl = options.UseSsl;
            var autoDelete = options.AutoDelete;

            configurator.AutoDelete = autoDelete;

            var activeMqHost = configurator.Host(hostName, port, h =>
            {
                h.Username(username);
                h.Password(password);
                if (useSsl)
                {
                    h.UseSsl();
                }
            });

            return activeMqHost;
        }

        private static void ConfigureRetryPolicy(IActiveMqReceiveEndpointConfigurator consumerConfigurator, RetryPolicyOptions retryPolicyOptions)
        {
            if (retryPolicyOptions != null)
            {
                var attempts = retryPolicyOptions.RetryAttempts;
                var interval = retryPolicyOptions.RetryIntervalMilliseconds;
                consumerConfigurator.UseMessageRetry(retryConfiguration =>
                    {
                        retryConfiguration.Interval(attempts, interval);
                    });
                consumerConfigurator.UseInMemoryOutbox();
            }
        }
    }
}