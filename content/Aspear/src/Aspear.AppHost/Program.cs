using Aspear.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var realmName = builder.Configuration["Keycloak:Realm"];
var keycloakUrl = builder.Configuration["Keycloak:PublicUrl"];
var grafanaUrl = builder.Configuration["Grafana:PublicUrl"];

// Storages
var cache = builder.AddRedis(ServiceNames.Cache);
var messaging = builder.AddRabbitMQ(ServiceNames.Messaging);
var storage = builder.AddAzureStorage(ServiceNames.Storage);
var blobs = storage.AddBlobs(ServiceNames.StorageBlobs);
if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(config =>
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            config.WithDataBindMount("C:\\Temp\\AzureStorage");
        else 
            config.WithDataBindMount("/tmp/AzureStorage");
    });
}

// Databases
var pgsql = builder
    .AddPostgres(ServiceNames.Postgres)
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();
// TODO: Create db when new nuget available
var appDb = pgsql
    .AddDatabase(ServiceNames.AppDatabase);
var keycloakDb = pgsql
    .AddDatabase(ServiceNames.KeycloakDatabase);

// Identity
var keycloakUsername = builder.AddParameter("keycloakUsername", "admin", true);
var keycloakPassword = builder.AddParameter("keycloakPassword", "@admin!", false);
var keycloak = builder.AddKeycloak(ServiceNames.Keycloak, port: 36008, adminUsername: keycloakUsername, adminPassword: keycloakPassword)
    .WithRealmImport("../Aspear.Keycloak/realms", true)
    .WithReference(keycloakDb)
    .WaitFor(pgsql)
    .WithExternalHttpEndpoints();

// API
var apiService = builder.AddProject<Projects.Aspear_ApiService>(ServiceNames.ApiService)
    .WithReference(keycloak)
    .WithReference(appDb)
    .WithReference(cache)
    .WithReference(messaging)
    .WithReference(blobs)
    .WaitFor(appDb)
    .WaitFor(cache)
    .WaitFor(messaging)
    .WaitFor(blobs)
    .WaitFor(keycloak)
    .WithHttpEndpoint(port: 36001, name: "http")
    .WithExternalHttpEndpoints()
    .WithEnvironment("KEYCLOAK_REALM", realmName)
    .WithEnvironment("KEYCLOAK_CLIENT_ID", builder.Configuration["Keycloak:BackClientId"])
    .WithEnvironment("KEYCLOAK_CLIENT_SECRET", builder.Configuration["Keycloak:BackClientSecret"]);


// Front-end
var nuxt = builder.AddNpmApp(ServiceNames.Nuxt, "../Aspear.Nuxt", "dev")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WaitFor(keycloak)
    .WithExternalHttpEndpoints()
    .WithHttpEndpoint(port: 36010, env: "PORT")
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_REALM", realmName)
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_SERVER_URL", keycloak.Resource.GetEndpoint("http"))
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_CLIENT_ID", builder.Configuration["Keycloak:FrontClientId"])
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_CLIENT_SECRET", builder.Configuration["Keycloak:FrontClientSecret"])
    .PublishAsDockerFile();

// Admin dashboard
var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithVolume("/var/lib/grafana")
    .WithBindMount("../Aspear.Grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../Aspear.Grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_ENABLED", "true")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_NAME", "Keycloak")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_CLIENT_ID", builder.Configuration["Keycloak:GrafanaClientId"])
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_CLIENT_SECRET", builder.Configuration["Keycloak:GrafanaClientSecret"])
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_AUTH_URL", $"{keycloakUrl}/realms/{realmName}/protocol/openid-connect/auth") // Publicly accessed by client
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_TOKEN_URL", $"{keycloak.Resource.GetEndpoint("http")}/realms/{realmName}/protocol/openid-connect/token")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_API_URL", $"{keycloak.Resource.GetEndpoint("http")}/realms/{realmName}/protocol/openid-connect/userinfo")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_ALLOW_SIGN_UP", "true")
    .WithEnvironment("GF_SERVER_ROOT_URL", grafanaUrl)
    .WaitFor(nuxt)
    .WaitFor(apiService)
    .WithHttpEndpoint(port: 36009, targetPort: 3000, name: "http")
    .WithExternalHttpEndpoints();


builder.Build().Run();
