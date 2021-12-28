using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.Models.CountryInfoList;
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
        private readonly IRequestClient<Student> _client;
        private readonly IRequestClient<LoginModel> _loginClient;
        private readonly IRequestClient<CountryInfo> _sharedClient;
        public StudentController(
            IBus busService,
            IRequestClient<Student> client,
            IRequestClient<LoginModel> loginClient,
            IRequestClient<CountryInfo> sharedClient
            )
        {
            _busService = busService;
            _client = client;
            _loginClient = loginClient;
            _sharedClient = sharedClient;
        }



        List<Student> students = new List<Student>()
        {
            new Student(){Id = 1, Age = 25, Name = "A", RegistrationNo = "001"},
            new Student(){Id = 2, Age = 25, Name = "A", RegistrationNo = "002"},
            new Student(){Id = 3, Age = 25, Name = "A", RegistrationNo = "003"},
            new Student(){Id = 4, Age = 25, Name = "A", RegistrationNo = "004"},
            new Student(){Id = 5, Age = 25, Name = "A", RegistrationNo = "005"},
            new Student(){Id = 6, Age = 25, Name = "A", RegistrationNo = "006"},
            new Student(){Id = 7, Age = 25, Name = "A", RegistrationNo = "007"},
            new Student(){Id = 8, Age = 25, Name = "A", RegistrationNo = "008"},
            new Student(){Id = 9, Age = 25, Name = "A", RegistrationNo = "009"},
            new Student(){Id = 11, Age = 25, Name = "A", RegistrationNo = "010"},
            new Student(){Id = 10, Age = 25, Name = "A", RegistrationNo = "011"},
        };




        [HttpPost("CreateStudent")]
        public async Task<Student> CreateStudent(Student student)
        {
            if (student != null)
            {
                //#region Command(Send)
                //Uri uri = new Uri("rabbitmq://localhost/studentQueue");
                //var endPoint = await _busService.GetSendEndpoint(uri);
                //await endPoint.Send(student);
                //return "true";
                //#endregion

                #region Request Response
                var request = _client.Create(student); // creating a request 
                var response = await request.GetResponse<Student>(); // Getting or requesting a response by Object type

                return await Task.FromResult<Student>(response.Message);
                #endregion
            }

            return await Task.FromResult<Student>(new Student());
        }



        [HttpPost("ResulCalc")]
        public async Task<string> ResultCalculation(Student student)
        {
            var request = _client.Create(student);
            var response = await request.GetResponse<Student>();

            return await Task.FromResult(response.Message.Status);
        }




        [HttpPost("StudentLogin")]
        public async Task<string> StudentLogin(LoginModel login)
        {
            var request = _loginClient.Create(login);
            var response = await request.GetResponse<LoginModel>();
            var message = response.Message;

            return await Task.FromResult(message.Message);
        }

        //[HttpGet("Countries")]
        //public async Task<List<CountryInfo>> Countries()
        //{
        //    CountryInfo country = new CountryInfo();
        //    var request = _sharedClient.Create(country);
        //    var response = await request.GetResponse<CountryInfo>();
        //    List<CountryInfo> countries = response.Message.countryInfos;
        //    return countries;

        //}





        [HttpGet("Countries")]
        public async Task<IActionResult> Countries()
        {
            CountryInfo country = new CountryInfo();
            var request = _sharedClient.Create(country);
            var response = await request.GetResponse<Countries>();
            return Ok(response);

        }








        #region Gutters

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
        #endregion
    }
}
