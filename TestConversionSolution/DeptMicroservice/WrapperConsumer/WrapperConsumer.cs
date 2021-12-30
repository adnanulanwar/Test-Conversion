using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeptMicroservice.WrapperConsumer
{
    // marker interface
    public interface IMyConsumerInterface
    {
    }
    public interface IMyMessageInterface
    {
    }

    public interface IMyConsumerInterface<T> : IMyConsumerInterface
        where T : IMyMessageInterface
    {
        Task Consume(T message);
    }



    public class WrapperConsumer<TConsumer, TMessage> : IConsumer<TMessage>
    where TMessage : class, IMyMessageInterface
    where TConsumer : IMyConsumerInterface<TMessage>
    {
        private readonly TConsumer _consumer;

        public WrapperConsumer(TConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task Consume(ConsumeContext<TMessage> context)
        {
            return _consumer.Consume(context.Message);
        }
    }
}
