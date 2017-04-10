using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TestDatabaseSeed.Models;
using MySQL.Data.Entity.Extensions;


namespace TestDatabaseSeed
{
  public enum DbTypeEnum
  {
    SqlServer,
    MySqlServer
  }

  public class TemporaryDbContextFactory : IDbContextFactory<DatabaseContext>
  {
    public DatabaseContext Create(DbContextFactoryOptions options)
    {
      var builder = new DbContextOptionsBuilder<DatabaseContext>();
      builder.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=testdb;Trusted_Connection=True;MultipleActiveResultSets=true");
      //builder.UseMySQL("server=localhost;userid=root;password=root!;database=test;");
      return new DatabaseContext(builder.Options);
    }
  }

  public sealed class DatabaseContext : DbContext
  {

    public DbSet<EntityTable> EntityTable { get; set; }

    private string ConnString { get; set; }
    private DbTypeEnum DbType { get; set; }

    private static bool _isConsole;


    /// <summary>
    /// Constructor used for context initialization 
    /// </summary>
    /// <param name="connString"></param>
    /// <param name="dbType"></param>
    public DatabaseContext(string connString, DbTypeEnum dbType)
    {
      SetDbInfo(connString, dbType);
    }
    /// <summary>
    /// Constructor used to for testing in console app.
    /// </summary>
    /// <param name="options"></param>
    public DatabaseContext(DbContextOptions options) : base(options)
    {
      _isConsole = true;
    }

    public int EnsureSeedData()
    {
      if (!AllMigrationsApplied())
      {
        throw new Exception("Migrations not applied.");
      }

      /*
      THIS DOES NOT WORK..
      

      EntityTable.AddRange(Models.EntityTable.EnsureSeedData());
      Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.EntityTable ON");
      var changeCount = SaveChanges();
      return changeCount;
     */

      // This works

      EntityTable.AddRange(Models.EntityTable.EnsureSeedData());
      Database.OpenConnection();
      Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.EntityTable ON");
      var changeCount = SaveChanges();
      Database.CloseConnection();
      return changeCount;


      
    }

    /// <summary>
    /// Returns whether or not all migrations have been applied or not.
    /// </summary>
    /// <returns></returns>
    public bool AllMigrationsApplied()
    {
      var applied = this.GetService<IHistoryRepository>()
        .GetAppliedMigrations()
        .Select(m => m.MigrationId);

      var total = this.GetService<IMigrationsAssembly>()
        .Migrations
        .Select(m => m.Key);

      return !total.Except(applied).Any();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!_isConsole)
      {
        switch (DbType)
        {
          case DbTypeEnum.MySqlServer:
            optionsBuilder.UseMySQL(ConnString);
            break;
          case DbTypeEnum.SqlServer:
            optionsBuilder.UseSqlServer(ConnString);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<EntityTable>(entity =>
      {
        entity.HasKey(e => e.Id)
          .HasName("PK_EntityTable");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Occur).HasColumnName("occur");
      });
      

    }

    private void SetDbInfo(string connString, DbTypeEnum dbType)
    {
      ConnString = connString;
      DbType = dbType;
    }
  }
}