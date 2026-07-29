# TaskTracker Azure Functions

A small task-tracking web app with a static frontend and a serverless C# API.

```text
app/  → browser UI, deployed by Azure Static Web Apps
api/  → Azure Functions (.NET 10 isolated worker)
```

## Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/api/tasks` | List tasks |
| `POST` | `/api/tasks` | Create a task |

## Run the API locally

```bash
cd api
dotnet run -- --port 7072
```

In a second terminal:

```bash
curl http://localhost:7072/api/tasks

curl -X POST http://localhost:7072/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Learn Azure Functions","description":"Build a serverless API"}'
```

Open `app/index.html` in a browser to view the simple frontend. It calls the API at `/api/tasks` after deployment.

## Storage configuration

Without a storage connection setting, the project uses in-memory tasks for local learning. To persist tasks in Azure Table Storage, set `TASKS_STORAGE_CONNECTION_STRING` as a local setting and as an Azure Static Web Apps environment variable. This value is a secret and must never be committed to Git.

Later steps will add Application Insights, Key Vault, and a database.

## Security note

`local.settings.json` is intentionally ignored by Git because it can contain local secrets. Never commit secrets or Azure connection strings.
