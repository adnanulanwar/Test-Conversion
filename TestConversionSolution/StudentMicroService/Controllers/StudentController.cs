using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IBus _busService;
        public StudentController(IBus busService)
        {
            _busService = busService;
        }



        [HttpPost("CreateStudent")]
        public async Task<string> CreateStudent(Student student)
        {
            if (student != null)
            {
                Uri uri = new Uri("rabbitmq://localhost/studentQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(student);
                return "true";
            }
            return "false";
        }


        [HttpGet]
        public IEnumerable<string> GetStudents()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
