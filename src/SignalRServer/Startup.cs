using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SignalRServer
{
    public class Startup
    {
        readonly string CorsPolicy = "CorsPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder =>
                    {
                        builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins(
                                "http://localhost:5000",
                                "http://localhost:4200")
                            .AllowCredentials();
                    });
            });

            services.AddControllers();
            services.AddSignalR();
            services.AddTransient<ExampleHub>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicy);
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ExampleHub>("/ws/example", options => 
                {
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
                });
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("SignalR server is running!");
                });
            });
        }
    }
}
