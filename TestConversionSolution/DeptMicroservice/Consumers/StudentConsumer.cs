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
            if(student.Marks > 50) 
            {
                student.Status = "Pass";
                
            }
            else
            {
                student.Status = "Fail";
            }
            
            return student;
        }
    }
}
