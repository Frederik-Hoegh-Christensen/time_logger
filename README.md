

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
# Ensure the docker container with an sql server is running since migrations will be applied on app startup
cd ./backend/src/WebAPI
dotnet build
dotnet run 