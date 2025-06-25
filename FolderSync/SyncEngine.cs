using System.Security.Cryptography;

public class SyncEngine
{
    private readonly Logger logger;

    public SyncEngine(Logger logger)
    {
        this.logger = logger;
    }


    // Synchronizes the source directory with the replica directory
    public void Synchronize(string sourceDir, string replicaDir)
    {
        if (!Directory.Exists(sourceDir)) throw new DirectoryNotFoundException("Source not found.");
        Directory.CreateDirectory(replicaDir);

        var sourceFiles = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
        var replicaFiles = Directory.GetFiles(replicaDir, "*", SearchOption.AllDirectories);

        // Copy files from source to replica
        try
        {
            foreach (var sourceFile in sourceFiles)
            {
                string relativePath = Path.GetRelativePath(sourceDir, sourceFile);
                string replicaFile = Path.Combine(replicaDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(replicaFile));


                if (!File.Exists(replicaFile) || GetMD5(sourceFile) != GetMD5(replicaFile))
                {
                    logger.Log($"Preparing to copy: {sourceFile} -> {replicaFile}");
                    File.Copy(sourceFile, replicaFile, true);
                    logger.Log($"Copied file: {relativePath}");
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.Log($"Access denied while copying files: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (IOException ex)
        {
            logger.Log($"IO error while copying files: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.Log($"Unexpected error while copying files: {ex.GetType().Name}: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }



        // Delte files that are in the replica but not in the source
        try
        {
            foreach (var replicaFile in replicaFiles)
            {
                string relativePath = Path.GetRelativePath(replicaDir, replicaFile);
                string sourceFile = Path.Combine(sourceDir, relativePath);

                if (!File.Exists(sourceFile))
                {
                    logger.Log($"Preparing to delete: {replicaFile}");
                    File.Delete(replicaFile);
                    logger.Log($"Deleted file: {relativePath}");
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.Log($"Access denied while deleting files: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (IOException ex)
        {
            logger.Log($"IO error while deleting files: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.Log($"Unexpected error while deleting files: {ex.GetType().Name}: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
    }


    // Calculates the MD5 hash of a file
    private string GetMD5(string filePath)
    {
        try
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.Log($"Access denied while calculating MD5 for file {filePath}: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (IOException ex)
        {
            logger.Log($"IO error while calculating MD5 for file {filePath}: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.Log($"Unexpected error while calculating MD5 for file {filePath}: {ex.GetType().Name}: {ex.Message}\nStack trace: {ex.StackTrace}");
            throw;
        }
    }
}