# FolderSynchronizer test project
Basic C# console application that performs **one-way folder synchronization** between a source and a replica directory.

### Requirements
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### CLI Usage

```bash
dotnet run --project FolderSync.csproj -- \
  --source "C:\\MySourceFolder" \
  --replica "D:\\MyReplicaFolder" \
  --interval 60 \
  --log "D:\\sync-log.txt"
