
public static class Logger
{
    private static readonly string LogFilePath = "ActionsLog.csv";

    public static void Log(User account, string action)
    {
        try
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{account.Name ?? "unknown"},{action}\n";

            
            if (!File.Exists(LogFilePath))
            {
                File.WriteAllText(LogFilePath, "Date,User,Action\n");
            }

            File.AppendAllText(LogFilePath, logMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logging failed: {ex.Message}");
        }
    }

    public static void LogError(string errorMessage)
    {
        Log(new User { Name = "System" }, $"ERROR: {errorMessage}");
    }

    internal static void Initialize()
    {
        throw new NotImplementedException();
    }
}