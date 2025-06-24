using System.CommandLine;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var sourceOption = new Option<DirectoryInfo>("--source", "Source folder path") { IsRequired = true };
        var replicaOption = new Option<DirectoryInfo>("--replica", "Replica folder path") { IsRequired = true };
        var intervalOption = new Option<int>("--interval", "Sync interval in seconds") { IsRequired = true };
        var logOption = new Option<FileInfo>("--log", "Log file path") { IsRequired = true };

        var rootCommand = new RootCommand("Folder Sync Tool")
        {
            sourceOption,
            replicaOption,
            intervalOption,
            logOption
        };

        static void SyncHandler(DirectoryInfo source, DirectoryInfo replica, int interval, FileInfo log)
        {
            var logger = new Logger(log.FullName);
            var engine = new SyncEngine(logger);

            while (true)
            {
                try
                {
                    engine.Synchronize(source.FullName, replica.FullName);
                    logger.Log("Synchronization completed.");
                    logger.Log($"Next synchronization check in {interval} seconds.");
                }
                catch (Exception ex)
                {
                    logger.Log("Error: " + ex.Message);
                }
                Thread.Sleep(interval * 1000);
            }
        }
        rootCommand.SetHandler(SyncHandler, sourceOption, replicaOption, intervalOption, logOption);

        return await rootCommand.InvokeAsync(args);
    }
}