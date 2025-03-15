# full_stack_template :rocket: :rocket: :rocket: 
## Before starting to code be sure to fork this repo to your own account since no push-restrictions have been set on this repo currently :smiley:
Welcome to this project template! This repository is designed to help you get started quickly with both the backend and frontend components already set up.
Take this template with a grain of salt and configure the general structure if you think it makes better sense - also be sure to consult any LLM for lightning fast development
# Backend 
## Required installation
any IDE 
.Net sdk 8.*.*

## Backend Architecture
The backend is structured using the Onion Architecture (Clean Architecture ish without the CQRS pattern), promoting separation of concerns and scalability. This structure ensures that core business logic remains independent of external frameworks and tools, allowing easier maintenance and testing.

### Key Features:
Core Layer: Contains the domain entities and application logic.
Application Layer: Handles use cases, services, and interfaces for communication between the core and infrastructure layers.
Infrastructure Layer: Implements data access using Entity Framework and is set up to work with PostgreSQL or MSSQL. Alternatively, you can use an SQLite in-memory database (or another in-memory database) for testing or lightweight development purposes.
Web API: The Startup Project is a Web API, providing API endpoints for client communication.
This setup allows flexibility in configuring your preferred database system while maintaining a clean separation between your business logic and data access layers.

# Frontend
The frontend of this project is built using TypeScript and React, with routing handled by react-router-dom v7
The initial folder structure has been configured but feel free to adjust it
 
## Required installation
Node 20^ (preferably use nvm to manage multiple version you may already have installed)

## A boilerplate readme can be found in the frontend directory for further information
