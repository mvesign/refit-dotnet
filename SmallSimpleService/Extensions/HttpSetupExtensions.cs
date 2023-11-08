using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Registry;
using Refit;
using SmallSimpleService.Models;
using SmallSimpleService.Settings;

namespace SmallSimpleService.Extensions;

/// <summary>
/// Available extensions on the <see cref="IServiceCollection" /> interface, for adding HTTP utilities.
/// </summary>
public static class HttpSetupExtensions
{
    /// <summary>
    /// Add HTTP policies to the given service collection.
    /// </summary>
    /// <param name="serviceCollection">Service collection.</param>
    /// <param name="httpPoliciesSettings">Settings related to setup the HTTP policies.</param>
    /// <returns>Service collection.</returns>
    internal static IServiceCollection AddHttpPolicies(
        this IServiceCollection serviceCollection, HttpPoliciesSettings httpPoliciesSettings
    )
    {
        // If retry policies are already configured, we want to add these to it instead of overwriting the existing ones.
        var policyRegistry =
            serviceCollection.BuildServiceProvider().GetService<IPolicyRegistry<string>>() ??
            serviceCollection.AddPolicyRegistry();

        // Only add the wait and retry policy when it's not added already.
        var retryKey = HttpPolicyKey.Retry.ToString();
        if (!policyRegistry.ContainsKey(retryKey))
            policyRegistry.Add(
                retryKey,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        Backoff.DecorrelatedJitterBackoffV2(
                            TimeSpan.FromSeconds(1), httpPoliciesSettings.WaitAndRetryCount
                        )
                    )
            );

        // Only add the circuit breaker policy when it's not added already.
        var circuitBreaker = HttpPolicyKey.CircuitBreaker.ToString();
        if (!policyRegistry.ContainsKey(circuitBreaker))
            policyRegistry.Add(
                circuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        httpPoliciesSettings.CircuitBreakerRetryCount,
                        httpPoliciesSettings.CircuitBreakerDuration
                    )
            );

        return serviceCollection;
    }
    
    /// <summary>
    /// Add HTTP client to the given service collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceCollection">Service collection.</param>
    /// <param name="httpClientSettings">HTTP client settings for one specific API.</param>
    /// <param name="jsonSerializerOptions">JSON serializer options.</param>
    /// <param name="httpPolicyKeys">Set of keys to setup http policies for the current HTTP client setup.</param>
    /// <returns>Service collection.</returns>
    internal static IServiceCollection AddHttpClient<T>(
        this IServiceCollection serviceCollection, HttpClientSettings httpClientSettings,
        JsonSerializerOptions jsonSerializerOptions, params HttpPolicyKey[] httpPolicyKeys
    ) where T : class
    {
        var refitSettings = jsonSerializerOptions != null
            ? new RefitSettings { ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions) }
            : null;

        var httpClientBuilder = serviceCollection.AddRefitClient<T>(refitSettings)
            .ConfigureHttpClient(httpClient => httpClient.ConfigureHttpClient(httpClientSettings));

        foreach (var httpPolicyKey in httpPolicyKeys)
            httpClientBuilder.AddPolicyHandlerFromRegistry(httpPolicyKey.ToString());

        return serviceCollection;
    }

    /// <summary>
    /// Configure the HTTP client.
    /// </summary>
    /// <param name="httpClient">Http client.</param>
    /// <param name="httpClientSettings">HTTP client settings for one specific API.</param>
    /// <exception cref="ArgumentException">Thrown when invalid HTTP client settings is given.</exception>
    private static void ConfigureHttpClient(this HttpClient httpClient, HttpClientSettings httpClientSettings)
    {
        if (httpClientSettings == null)
            throw new ArgumentException(
                $"Invalid HTTP API settings present, expected of type '{typeof(HttpClientSettings)}'",
                nameof(httpClientSettings));

        if (!Uri.TryCreate(httpClientSettings.BaseUrl, UriKind.Absolute, out var absoluteBaseUrl))
            throw new ArgumentException(
                $"Invalid absolute URL present in '{nameof(httpClientSettings.BaseUrl)}'",
                nameof(httpClientSettings));

        httpClient.BaseAddress = absoluteBaseUrl;

        // ReSharper disable once InvertIf
        if (httpClientSettings.Headers is { Count: > 0 })
            foreach (var (headerKey, headerValue) in httpClientSettings.Headers)
                httpClient.DefaultRequestHeaders.Add(headerKey, headerValue);
    }
}