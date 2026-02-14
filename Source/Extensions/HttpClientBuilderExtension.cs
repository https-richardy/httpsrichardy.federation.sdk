namespace HttpsRichardy.Federation.Sdk.Extensions;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddSdkHttpClient<TClient, TImplementation>(this IServiceCollection services, string baseUrl)
        where TClient : class
        where TImplementation : class, TClient
    {
        return services.AddHttpClient<TClient, TImplementation>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });
    }

    public static IHttpClientBuilder WithRealmInterceptor(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<RealmInterceptor>();
    }

    public static IHttpClientBuilder WithAuthenticationInterceptor(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<AuthenticationInterceptor>();
    }
}
