namespace HttpsRichardy.Federation.Sdk.Extensions;

public static class FederationExtension
{
    public static void AddFederation(this IServiceCollection services, Action<FederationOptions> configure)
    {
        var options = new FederationOptions();

        configure(options);

        services.AddTransient<AuthenticationInterceptor>();
        services.AddTransient<RealmInterceptor>();

        services.AddSingleton(options);
        services.AddHttpClient<IConnectClient, ConnectClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddSdkHttpClient<IIdentityClient, IdentityClient>(options.BaseUrl)
            .WithRealmInterceptor()
            .WithAuthenticationInterceptor();

        services.AddSdkHttpClient<IPermissionsClient, PermissionsClient>(options.BaseUrl)
            .WithRealmInterceptor()
            .WithAuthenticationInterceptor();

        services.AddSdkHttpClient<IGroupsClient, GroupsClient>(options.BaseUrl)
            .WithRealmInterceptor()
            .WithAuthenticationInterceptor();

        services.AddSdkHttpClient<IRealmsClient, RealmsClient>(options.BaseUrl)
            .WithRealmInterceptor()
            .WithAuthenticationInterceptor();

        services.AddSdkHttpClient<IUsersClient, UsersClient>(options.BaseUrl)
            .WithRealmInterceptor()
            .WithAuthenticationInterceptor();

        services.AddBearerAuthentication();
        services.AddAuthorization();
    }
}