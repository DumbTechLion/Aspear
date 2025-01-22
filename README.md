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
### Configure Databases
1. Start AppHost
2. On Aspire Dashboard, access to pgsql-admin URL.
3. On the pgsql server, create two databases:
- app-db
- keycloak-db
4. Restart AppHost

### Configure Keycloak
1. Start AppHost.
2. Login in Keycloak with default login (admin:@admin!). **If in Production, change this password!!! Generate a random one and save it safely (in a password manager for example).**
3. Go to Aspear realm
4. Create a new admin user with realm-admin role
5. Clients should be already configured (!!!!In production, regenerate Credentials like Client Secrets!!!!)

### Configure Grafana
1. Log into Grafana with default credentials (admin:admin)
2. Change default password to whatever you want
3. Log out
4. Log in with Keycloak as the recently created admin on Keycloak (in Realm Aspear)
5. Log out
6. Log in with the Grafana admin account
7. Go to Users and go to Keycloak admin
8. Set it as Grafana Admin
9. (Optional) You can now delete the default Grafana Admin if needed (or in Production)

## First Deployment to Production
### First deployment

### Configure Keycloak

### After first deployment and configuration
1. For non-secrets, fill out AppHost appsettings.Production.json (NON-SECRET ONLY)
2. For secrets, add them in your github secrets and ensure it is deployed (see .github/deploy.yml)
3. Redeploy. 

To check if everything worked, you should be able to see the new secrets on your new Aspire container.

## Troubleshoot
### Deprecation warnings as errors when starting AppHost
This error is due to a common package deprecation since node 21.

Use nvm to target node 20 in your IDE terminal. It will take the latest node 20.
