# TaskManager

This repo is API for https://github.com/MFGod/task-manager.git

## Building

### Source code

To run project clone the repository:

```bash
git clone https://github.com/TWR-pixel/TaskManager.git

cd TaskManager
```

Then set environment variables:

```bash
set JWT_SECRET_KEY=DEFAULT_VALUE
set EMAIL_SENDER_API_KEY=<INSERT_YOUR_API_KEY>
```

Then run:

```bash
dotnet restore TaskManager.sln
```

Then build all projects:

```bash
dotnet build TaskManager.sln
```

To run API project:

```bash
dotnet run --project src/PublicApi/TaskManager.PublicApi/TaskManager.PublicApi.csproj
```

### Docker

```bash
docker compose-up
```
