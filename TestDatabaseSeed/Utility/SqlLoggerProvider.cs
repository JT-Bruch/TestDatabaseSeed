using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace TestDatabaseSeed.Utility
{
  public class SqlLoggerProvider : ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName)
    {
      if (categoryName == typeof(IRelationalCommandBuilderFactory).FullName)
      {
        return new SqlLogger();
      }

      return new NullLogger();
    }

    public void Dispose()
    { }

    private class SqlLogger : ILogger
    {
      public bool IsEnabled(LogLevel logLevel)
      {
        return true;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      {
        if (eventId.Id != (int)RelationalEventId.ExecutedCommand) return;
        var data = state as IEnumerable<KeyValuePair<string, object>>;

        if (data == null) return;

        var commandText = data.Single(p => p.Key == "CommandText").Value;
        Console.WriteLine(commandText);
      }

      public IDisposable BeginScope<TState>(TState state)
      {
        return null;
      }
    }

    private class NullLogger : ILogger
    {
      public bool IsEnabled(LogLevel logLevel)
      {
        return false;
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      { }

      public IDisposable BeginScope<TState>(TState state)
      {
        return null;
      }
    }
  }
}
