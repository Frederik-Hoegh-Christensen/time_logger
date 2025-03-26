

# Required installation
.Net sdk 9.*.*

Docker

Node 20^

# How to run

## Database
cd./backend

docker compose up

## Frontend
cd ./frontend

npm install

npm run dev

## Backend
### Ensure the docker command succeeded since migrations will be applied onto the container on app startup
cd ./backend/src/WebAPI

dotnet build

dotnet run 