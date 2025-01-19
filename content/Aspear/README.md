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
2. Login in Keycloak with default login (admin:@admin!). **If in Production, change this password!!! Generate a random one and save it safely (in a password manager for example).**
3. Create a realm (default name "AspireKeycloak")
4. Create a new user if needed
5. Create new clients: api-client, front-client, grafana-client
6. Add the client secrets to your AppHost project local secrets:
   - Keycloak:BackClientSecret (default name "AspireKeycloak")
   - Keycloak:BackFrontSecret (default name "api-services")
   - Keycloak:BackGrafanaSecret
7. Restart the AppHost. The front and the back should be configured for authentication!

## First Deployment to Production
### First deployment

### Configure Keycloak

### After first deployment and configuration
1. For non-secrets, fill out AppHost appsettings.Production.json (NON-SECRET ONLY)
2. For secrets, add them in your github secrets and ensure it is deployed (see .github/deploy.yml)
3. Redeploy. 

To check if everything worked, you should be able to see the new secrets on your new Aspire container.