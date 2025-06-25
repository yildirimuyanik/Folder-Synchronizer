# FolderSynchronizer test project
Basic C# console application that performs **one-way folder synchronization** between a source and a replica directory.

### Requirements
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### How to run the application

Clone the repository
```bash
git clone[https://github.com/yildirimuyanik/Folder-Synchronizer.git]
```

Go to the project's root folder 
```bash
dotnet run 
  --source C:\\MySourceFolder 
  --replica C:\\MyReplicaFolder
  --interval 60 
  --log C:\\MysyncLog.txt
```
