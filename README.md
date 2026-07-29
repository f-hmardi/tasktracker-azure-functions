# TaskTracker Functions

Serverless C# API built with Azure Functions (.NET 10 isolated worker).

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/tasks` | List tasks |
| `POST` | `/api/tasks` | Create a task |

## Run locally

```bash
dotnet run -- --port 7072
```

In a second terminal:

```bash
curl http://localhost:7072/api/tasks

curl -X POST http://localhost:7072/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn Azure Functions","description":"Build a serverless API"}'
```

## Current architecture

The project uses an in-memory task store for learning. Tasks disappear when the local Function stops. Later steps will add Azure Storage, Application Insights, Key Vault, and a database.

## Security note

`local.settings.json` is intentionally ignored by Git because it can contain local secrets. Never commit secrets or Azure connection strings.
