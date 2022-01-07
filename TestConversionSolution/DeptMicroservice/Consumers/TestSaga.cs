using MassTransit.Saga;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeptMicroservice.Consumers
{

    public class TestSaga : ISaga, InitiatedBy<Teacher>, Orchestrates<Student>
    {
        public Guid CorrelationId { get; set; }
        public DateTime Submitted { get; set; }
        public DateTime Completed { get; set; }

        public async Task Consume(ConsumeContext<Teacher> context)
        {
            Submitted = context.Message.OrderDate;
        }

        public async Task Consume(ConsumeContext<Student> context)
        {
            Completed = context.Message.CompletionDate;
        }
    }

}
