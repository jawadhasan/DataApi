using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using SampleApi.Data;
using SampleApi.Web.Helpers;
using SampleApi.Web.Hubs;
using SampleApi.Web.Services.User;

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
           

            services.AddSingleton(new Random());
            services.AddSingleton<OrderChecker>();
            services.AddHttpContextAccessor();
            services.AddSignalR();


            //local users
            var users = new Dictionary<string, string> {{"test", "test123"}, {"admin", "admin123"}};
            services.AddSingleton<IUserService>(new BasicUserService(users));
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(InMemoryConfig.SecretKey)),
                    ValidIssuer = InMemoryConfig.Issuer,
                    ValidAudience = InMemoryConfig.Audience,

                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    //ValidateLifetime = true,
                    //ValidateIssuerSigningKey = true

                };
            });



            services.AddMvc();
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

            app.UseAuthentication();
            app.UseMvc();

            dbseeder.SeedAsync(app.ApplicationServices).Wait();
        }
    }
}
