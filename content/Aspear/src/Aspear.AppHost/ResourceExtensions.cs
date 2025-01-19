namespace Aspear.AppHost;

public static class ResourceExtensions
{
    public static IResourceBuilder<KeycloakResource> WithReference(this IResourceBuilder<KeycloakResource> builder, IResourceBuilder<PostgresDatabaseResource> source)
    {
        var database = source.Resource;
        var server = database.Parent;

        return builder.WithEnvironment(context =>
        {
            var primaryEndpoint = server.PrimaryEndpoint;
            context.EnvironmentVariables["KC_DB"] = "postgres";
            context.EnvironmentVariables["KC_DB_URL_HOST"] = primaryEndpoint.ContainerHost;
            context.EnvironmentVariables["KC_DB_URL_PORT"] = ReferenceExpression.Create($"{primaryEndpoint.Property(EndpointProperty.Port)}");
            context.EnvironmentVariables["KC_DB_URL_DATABASE"] = database.DatabaseName; // Keycloak does not create the database by itself. Using the default database for the time being.
            context.EnvironmentVariables["KC_DB_SCHEMA"] = "public"; // Keycloak does not create the schema by itself. Using default schema for the time being.
            context.EnvironmentVariables["KC_DB_USERNAME"] = (object?)server.UserNameParameter ?? "postgres";
            context.EnvironmentVariables["KC_DB_PASSWORD"] = server.PasswordParameter;
        });
    }
}