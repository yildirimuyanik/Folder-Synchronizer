public class Logger
{
    private readonly string logPath;

    public Logger(string path)
    {
        logPath = path;
    }

    public void Log(string message)
    {
        string fullMessage = DateTime.Now + ": " + message;
        Console.WriteLine(fullMessage);
        File.AppendAllText(logPath, fullMessage + Environment.NewLine);
    }
}