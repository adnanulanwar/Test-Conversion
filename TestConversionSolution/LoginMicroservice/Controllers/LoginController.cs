using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IBus _busService;
        private readonly IRequestClient<LoginModel> _client;
        public LoginController(IBus busService, IRequestClient<LoginModel> client)
        {
            _busService = busService;
            _client = client;
        }

        //[HttpPost("StudentLogin")]
        //public async Task<string> StudentLogin(LoginModel login)
        //{
        //    var request = _client.Create(login);
        //    var response = await request.GetResponse<LoginModel>();

        //    return await Task.FromResult(response.Message.Message);
        //}

    }
}
