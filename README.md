# TaskManager

This repo is API for https://github.com/MFGod/aera.git

## Building

### Source code

To run project clone the repository:

```bash
git clone https://github.com/TWR-pixel/TaskManager.git

cd TaskManager
```
Read this docs to get application code for mail.yandex.ru https://yandex.ru/support/id/ru/authorization/app-passwords
Then set this user secrets variables:

To create google client secret and client id (https://console.cloud.google.com/)
```bash
dotnet user-secrets set "Google:ClientSecret" "YourClientSecret"
dotnet user-secrets set "Google:ClientId" "YourClientId"
dotnet user-secrets set "Google:ServerRedirectUri" "https://localhost:7049/api/oauth/google-callback"
dotnet user-secrets set "Google:ClientRedirectUri" "http://localhost:3000"
dotnet user-secrets set "TmMailerooApiKey" "YourApiKey"
dotnet user-secrets set "TmJwtSecretKey" "AWOIEJFPOAIWEJFJEWPIOFJWPEOIJFFOIJWEPOIFJWEOPIJFIO"
dotnet user-secrets set "TmEmailApiKey" "YouEmailApiKey"

```

For confirmation by mail to work, insert your email address here:
```json
{
  "EmailSenderOptions": {
    "EmailFrom": "<COPY-YOUR-ADDRESS-HERE>",
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
