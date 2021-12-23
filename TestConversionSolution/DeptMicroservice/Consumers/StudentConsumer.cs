using MassTransit;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeptMicroservice.Consumers
{
    public class StudentConsumer : IConsumer<Student>
    {
        public Task Consume(ConsumeContext<Student> context)
        {
            var stdObj = context.Message;
            return Task.FromResult(stdObj);
        }
    }
}
