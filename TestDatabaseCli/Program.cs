using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TestDatabaseSeed;
using TestDatabaseSeed.Utility;

namespace TestDatabaseCli
{
  internal class Program
  {
    static void Main(string[] args)
    {
      const string connString = "data source=DEV-JTBRUCH\\SQLEXPRESS;initial catalog=Sandbox2;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

      using (var dbContext = new DatabaseContext(connString, DbTypeEnum.SqlServer) )
      {
        dbContext.Database.EnsureDeleted();
        var serviceProvider = dbContext.GetInfrastructure<IServiceProvider>();
        var logFactory = serviceProvider.GetService<ILoggerFactory>();
        logFactory.AddProvider(new SqlLoggerProvider());

        dbContext.Database.Migrate();

        Console.WriteLine($"Number of items added: {dbContext.EnsureSeedData()}");
        Console.ReadLine();
      }
    }
  }
}