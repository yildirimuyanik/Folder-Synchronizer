class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Set the following params: FolderSyncTool <source> <replica> <intervalSeconds> <logFilePath>");
            return;
        }

        string source = args[0];
        string replica = args[1];
        int interval = int.Parse(args[2]);
        string logFile = args[3];

        var logger = new Logger(logFile);
        var engine = new SyncEngine(logger);

        while (true)
        {
            try
            {
                engine.Synchronize(source, replica);
                logger.Log("Synchronization complete.");
                logger.Log($"Next synchronization check in {interval} seconds.");
            }
            catch (Exception ex)
            {
                logger.Log("Error: " + ex.Message);
            }
            Thread.Sleep(interval * 1000);
        }
    }
}