using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeacherMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IRequestClient<LoginModel> _loginClient;

        public TeacherController(IBus bus, IRequestClient<LoginModel> loginClient)
        {
            _bus = bus;
            _loginClient = loginClient;
        }

        [HttpPost("TeacherLogin")]
        public async Task<string> TeacherLogin(LoginModel login)
        {
            var request = _loginClient.Create(login);
            var response = await request.GetResponse<LoginModel>();
            var message = response.Message;

            return await Task.FromResult(message.Message);
        }
    }
}
