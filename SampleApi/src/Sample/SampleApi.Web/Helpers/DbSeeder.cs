using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleApi.Core;
using SampleApi.Data;

namespace SampleApi.Web.Helpers
{
    public class DbSeeder
    {
        readonly ILogger _Logger;

        public DbSeeder(ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger("DbSeederLogger");
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            //Based on EF team's example at https://github.com/aspnet/MusicStore/blob/dev/samples/MusicStore/Models/SampleData.cs
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<AppDbContext>();
                if (await db.Database.EnsureCreatedAsync())
                {
                    if (!await db.Companies.AnyAsync())
                    {
                        await InsertSampleData(db);
                    }
                }
            }
        }

        public async Task InsertSampleData(AppDbContext db)
        {
            var states = GetStates();
            db.States.AddRange(states);
            db.Companies.AddRange(_Companies);
            try
            {
                int numAffected = await db.SaveChangesAsync();
                _Logger.LogInformation(@"Saved {numAffected} states");
            }
            catch (Exception exp)
            {
                _Logger.LogError($"Error in {nameof(DbSeeder)}: " + exp.Message);
                throw;
            }
        }



        private List<State> GetStates()
        {
            var states = new List<State>
            {
                new State { Name = "Alabama"},
                new State { Name = "Montana"},
                new State { Name = "Alaska"},
                new State { Name = "Nebraska"},
                new State { Name = "Arizona" }
            };

            return states;
        }
        private List<Company> _Companies = new List<Company>
    {
      new Company{Id = 1, Name = "Company1", IsActive = true},
      new Company{Id = 2, Name = "Company2", IsActive = false},
      new Company{Id = 3, Name = "Company3", IsActive = true}
    };

    }
}
