# TaskManager

This repo is API for https://github.com/MFGod/task-manager.git

## Building

### Source code

To run project clone the repository:

```bash
git clone https://github.com/TWR-pixel/TaskManager.git

cd TaskManager
```
Read this docs to get application code for mail.yandex.ru https://yandex.ru/support/id/ru/authorization/app-passwords
Then set this environment variables:

```bash
set JWT_SECRET_KEY=DEFAULT_VALUE
set EMAIL_SENDER_API_KEY=<INSERT_YOUR_APPLICATION_CODE>
```

For confirmation by mail to work, insert your email address here (don't forget to create an environment variable EMAIL_SENDER_API_KEY with your key value):
```json
{
  "EmailSenderOptions": {
    "EmailFrom": "<COPY-YOU-ADDRESS-HERE>",
    "Port": "465",
    "Host": "smtp.yandex.ru"
  }
}

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
