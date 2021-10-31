using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Server;

namespace MQTT_DOTNET
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
            services.AddControllers();

            var optionsBuilder = new MqttServerOptionsBuilder()
                    .WithApplicationMessageInterceptor(context =>
                    {               
                        if (context.ApplicationMessage.Topic == "teste/topic1")
                        {
                            String inPayload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload,0,context.ApplicationMessage.Payload.Length); 
                            Console.WriteLine("Mensagem Recebida!! {0}",inPayload);
                            //context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes("The server injected payload.");
                        }

      
                    });

            services.AddHostedMqttServer(optionsBuilder.Build())
                    .AddMqttConnectionHandler()
                    .AddConnections();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseMqttServer(server =>{});

        }
    }
}
