using MassTransit;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeptMicroservice.Consumers
{
    public class StudentConsumer<T> : IConsumer<Student>, IConsumer<Teacher>
    {
        public async Task Consume(ConsumeContext<Student> context)
        {
            var stdObj = context.Message;
            //return Task.FromResult(stdObj);
            Student student = new Student();
            student.Id = 1;
            student.Name = stdObj.Name;
            student.Age = stdObj.Age;
            student.RegistrationNo = "001";



            await context.RespondAsync(Calc(stdObj));
        }
        public Student Calc(Student student)
        {
            if (student.Marks > 50)
            {
                student.Status = "Pass";

            }
            else
            {
                student.Status = "Fail";
            }

            return student;
        }

        public Task Consume(ConsumeContext<Teacher> context)
        {
            throw new NotImplementedException();
        }





    }

    public interface IMessage
    {
    }

    public interface IEvent
       : IMessage
    {
    }

    public interface IEventConsumer<in TEvent>
        where TEvent : IEvent
    {
        Task Consume(TEvent @event);
    }



    public class EventConsumerAdapter<TEvent> : IConsumer<TEvent> where TEvent : class, IEvent
    {
        private readonly IEventConsumer<TEvent> _eventConsumer;
        public EventConsumerAdapter(IEventConsumer<TEvent> eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var message = context.Message;
            await _eventConsumer.Consume(message);
        }

        //public async Task Consume(ConsumeContext<TEvent> context)
        //{
        //    var message = context.Message;
        //    await _eventConsumer.Consume(message);
        //}

    }


}
