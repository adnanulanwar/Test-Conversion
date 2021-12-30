using GreenPipes;
using MassTransit;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginMicroservice.Consumers
{
    public class LoginConsumer : IConsumer<LoginModel>
    {
        public async Task Consume(ConsumeContext<LoginModel> context)
        {
            LoginModel loginObj = context.Message;
            LoginModel loginReturn = LoginMethod(loginObj);
            await context.RespondAsync(loginReturn);
        }

        private LoginModel LoginMethod(LoginModel login)
        {
            LoginModel model = new LoginModel();
            if (model.Username == login.Username && model.Password == login.Password)
            {
                login.Message = "Login Successful";
            }
            else
            {
                login.Message = "Login Failed";
            }
            return login;
        }
    }



    //public class MyConsumeFilter<T> :
    //IFilter<ConsumeContext<T>>
    //where T : class
    //{
    //    public void Probe(ProbeContext context)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public MyConsumeFilter(IMyDependency dependency) { }

    //}

}
