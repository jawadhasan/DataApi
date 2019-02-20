using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Data;
using SampleApi.Web.Helpers;
using SampleApi.Web.Hubs;

namespace SampleApi.Web
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

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowAnyOrigin()
                    .WithOrigins("http://localhost:4200") //https://trailheadtechnology.com/breaking-change-in-aspnetcore-2-2-for-signalr-and-cors/
                    .AllowCredentials();
            }));


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
            });
            services.AddScoped<DbSeeder>();
            services.AddMvc();

            services.AddSingleton(new Random());
            services.AddSingleton<OrderChecker>();
            services.AddHttpContextAccessor();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbSeeder dbseeder)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<CoffeeHub>("/coffeeHub");
            });

            app.UseMvc();

            dbseeder.SeedAsync(app.ApplicationServices).Wait();
        }
    }
}
