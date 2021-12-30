using DeptMicroservice.Consumers;
using DeptMicroservice.WrapperConsumer;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration.MultiBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeptMicroservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var interfaceType = typeof(IEvent);
            var consumerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToList();

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur =>
                {
                    //cur.UseHealthCheck(provider);
                    cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cur.ReceiveEndpoint("loginQueueresponse", oq =>
                    {
                        oq.PrefetchCount = 20;
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                    });
                }));
            });
            services.AddSingleton(typeof(StudentConsumer<>), typeof(IConsumer<>));
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeptMicroservice", Version = "v1" });
            });
        }



        //public IServiceCollectionConfigurator ConfigureMassTransitConsumers(this IServiceCollectionConfigurator serviceConfigurator, Assembly assembly)
        //{
        //    foreach (var type in assembly.GetTypes())
        //    {
        //        var attributes = type.GetCustomAttributes(typeof(RegisterCommandConsumerAttribute), false);
        //        if (attributes.Length <= 0) continue;
        //        foreach (var attribute in attributes)
        //        {
        //            if (attribute is RegisterCommandConsumerAttribute registerCommandConsumerAttribute)
        //            {
        //                Type consumerType = typeof(CommandConsumer<>).MakeGenericType(registerCommandConsumerAttribute.CommandType);
        //                Type consumerDefinitionType = typeof(CommandConsumerDefinition<>).MakeGenericType(registerCommandConsumerAttribute.CommandType);
        //                serviceConfigurator.AddConsumer(consumerType, consumerDefinitionType);
        //            }
        //        }
        //    }
        //    return serviceConfigurator;
        //}









        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeptMicroservice v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
