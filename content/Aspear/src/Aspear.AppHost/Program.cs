using Aspear.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

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

var pgsql = builder
    .AddPostgres(ServiceNames.Postgres)
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();
var appDb = pgsql
    .AddDatabase(ServiceNames.AppDatabase);
var keycloakDb = pgsql
    .AddDatabase(ServiceNames.KeycloakDatabase);

var realmName = builder.Configuration["Keycloak:Realm"];
var keycloakUrl = builder.Configuration["Keycloak:Url"];
var keycloakUsername = builder.AddParameter("keycloakUsername", "admin", true);
var keycloakPassword = builder.AddParameter("keycloakPassword", "@admin!", false);
var keycloak = builder.AddKeycloak(ServiceNames.Keycloak, adminUsername: keycloakUsername, adminPassword: keycloakPassword)
    .WithReference(keycloakDb)
    .WithHttpEndpoint(targetPort: 8080, name: "http") 
    .WithExternalHttpEndpoints();

var apiService = builder.AddProject<Projects.Aspear_ApiService>(ServiceNames.ApiService)
    .WithExternalHttpEndpoints()
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
    .WithEnvironment("KEYCLOAK_REALM", realmName)
    .WithEnvironment("KEYCLOAK_CLIENT_ID", builder.Configuration["Keycloak:BackClientId"])
    .WithEnvironment("KEYCLOAK_CLIENT_SECRET", builder.Configuration["Keycloak:BackClientSecret"]);

var nuxt = builder.AddNpmApp(ServiceNames.Nuxt, "../Aspear.Nuxt", "dev")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService)
    .WaitFor(keycloak)
    .WithHttpEndpoint(env: "PORT")
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_REALM", realmName)
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_SERVER_URL", keycloak.Resource.GetEndpoint("http"))
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_CLIENT_ID", builder.Configuration["Keycloak:FrontClientId"])
    .WithEnvironment("NUXT_OAUTH_KEYCLOAK_CLIENT_SECRET", builder.Configuration["Keycloak:FrontClientSecret"])
    .PublishAsDockerFile();

var grafanaUrl = builder.Configuration["Grafana:Url"];
var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../Aspear.Grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../Aspear.Grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_ENABLED", "true")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_NAME", "Keycloak")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_CLIENT_ID", builder.Configuration["Keycloak:GrafanaClientId"])
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_CLIENT_SECRET", builder.Configuration["Keycloak:GrafanaClientSecret"])
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_AUTH_URL", $"{keycloakUrl}/realms/{builder.Configuration["Keycloak:Realm"]}/protocol/openid-connect/auth")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_TOKEN_URL", $"{keycloakUrl}/realms/{builder.Configuration["Keycloak:Realm"]}/protocol/openid-connect/token")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_API_URL", $"{keycloakUrl}/realms/{builder.Configuration["Keycloak:Realm"]}/protocol/openid-connect/userinfo")
    .WithEnvironment("GF_AUTH_GENERIC_OAUTH_ALLOW_SIGN_UP", "false")
    .WithEnvironment("GF_SERVER_ROOT_URL", grafanaUrl)
    .WaitFor(nuxt)
    .WaitFor(apiService)
    .WithHttpEndpoint(port: 6110, targetPort: 3000, name: "http")
    .WithExternalHttpEndpoints();


builder.Build().Run();