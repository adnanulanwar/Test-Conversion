using DeptMicroservice.Consumers;
using System;
using System.Collections.Generic;

namespace DiDrDe.MessageBus.Infra.MassTransit.Configuration
{
    public class EventTypesOptions
    {
        private readonly List<Type> _eventTypes;
        public IReadOnlyList<Type> EventTypes => _eventTypes.AsReadOnly();

        public EventTypesOptions()
        {
            _eventTypes = new List<Type>();
        }

        public EventTypesOptions ConsumesEvent<TEventType>()
            where TEventType : IEvent
        {
            var typeSupplied = typeof(TEventType);

            _eventTypes.Add(typeSupplied);

            return this;
        }
    }
}