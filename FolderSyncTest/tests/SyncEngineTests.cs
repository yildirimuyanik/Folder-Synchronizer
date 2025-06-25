namespace FolderSyncTest.tests
{
    [TestFixture]
    public class SyncEngineTests
    {
        private string sourceDir;
        private string replicaDir;
        private string logPath;

        [SetUp]
        public void Setup()
        {
            sourceDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            replicaDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(sourceDir);
            Directory.CreateDirectory(replicaDir);
            logPath = Path.Combine(replicaDir, "log.txt");
        }

        [Test]
        public void SyncEngine_CopyNewFile_CheckContent()
        {

            File.WriteAllText(Path.Combine(sourceDir, "test.txt"), "YildirimTest");

            var logger = new Logger(logPath);
            var engine = new SyncEngine(logger);

            engine.Synchronize(sourceDir, replicaDir);

            var copied = Path.Combine(replicaDir, "test.txt");
            Assert.That(File.Exists(copied), Is.True, "File should exist in the replica directory.");
            Assert.That(File.ReadAllText(copied), Is.EqualTo("YildirimTest"), "File content should match.");
        }

        [Test]
        public void SyncEngine_UpdateFile_WhenContentIsDifferent()
        {
            File.WriteAllText(Path.Combine(sourceDir, "file.txt"), "YLD_newContent");
            File.WriteAllText(Path.Combine(replicaDir, "file.txt"), "YLD_oldContent");

            var logger = new Logger(logPath);
            var engine = new SyncEngine(logger);
            engine.Synchronize(sourceDir, replicaDir);

            var updatedFilePath = Path.Combine(replicaDir, "file.txt");

            Assert.That(File.ReadAllText(updatedFilePath), Is.EqualTo("YLD_newContent"), "File content should be updated to 'YLD_newContent'");
        }

        [Test]
        public void SyncEngine_DeleteFile_IfMissingInSourceFolder()
        {
            var file = Path.Combine(replicaDir, "delete.txt");
            File.WriteAllText(file, "YLD_Delete");

            var logger = new Logger(logPath);
            var engine = new SyncEngine(logger);
            engine.Synchronize(sourceDir, replicaDir);

            Assert.That(File.Exists(file), Is.False, "File should be deleted if it is missing in the source folder.");
        }

        [TearDown]
        public void Cleanup()
        {
            Directory.Delete(sourceDir, true);
            Directory.Delete(replicaDir, true);
        }
    }
}
