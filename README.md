# Aspear

Solution used for conventions and other events to organize and register their attendees

## Dependencies
This solution uses the following dependencies:
- .NET 9.0
- Nuxt 3
- PostgreSQL 
- RabbitMQ
- Redis
- Keycloak
- Azure Storage (with emulator on local)

## Setup
### Configure Keycloak
1. Start AppHost.
2. Login in Keycloak with default login (admin:@admin!). **If in Production, change this password!!! Generate a random one and save it safely (on a password manager for example).**
3. Create a realm (default name "AspireKeycloak")
4. Create a new client (default name "api-services") with Client ID/Secret Authentication 
and save client ID and SECRET aside
5. Create a new user if needed
6. Open your project (AppHost) local secrets and set
   - KEYCLOAK_REALM (default name "AspireKeycloak")
   - KEYCLOAK_CLIENT_ID (default name "api-services")
   - KEYCLOAK_CLIENT_SECRET
7. Restart the AppHost. The front and the back should be configured for authentication!