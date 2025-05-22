
using System;
using System.IO;


namespace EasyTrackerAPI.Implementation
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger()
        {
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs.txt");

            
            if (!File.Exists(_logFilePath))
            {
                File.WriteAllText(_logFilePath, "Log started at " + DateTime.Now + "\n");
            }
        }

        public void LogInformation(string message)
        {
            Log("INFO", message);
        }

        public void LogWarning(string message)
        {
            Log("WARN", message);
        }

        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        private void Log(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}\n";
            File.AppendAllText(_logFilePath, logEntry);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}