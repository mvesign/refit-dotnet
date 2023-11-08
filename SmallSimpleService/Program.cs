using System;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmallSimpleService;
using SmallSimpleService.Extensions;
using SmallSimpleService.Models;
using SmallSimpleService.RefitClients;
using SmallSimpleService.Services;
using SmallSimpleService.Settings;

Console.WriteLine("Hello, World!");

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));
    })
    .ConfigureServices((context, services) =>
    {
        // Before anything, we must determine how the JSON serialization is being done by Refit.
        // This can be done by using the internal 'JsonSerializerOptions' class.
        var jsonSerializerOptions = new JsonSerializerOptions();

        // Add the wrapper service
        services.AddSingleton<SmallSimpleApiService>();

        // Setup a predefined HTTP policies to be usable for any HTTP clients.
        services.AddHttpPolicies(context.Configuration.GetSection("HttpPolicies").Get<HttpPoliciesSettings>());
        
        // Setup the RefitApiClient interface as an HTTP client, with the predefined HTTP policies.
        services.AddHttpClient<ISmallSimpleApiClient>(
            context.Configuration.GetSection("SmallSimpleApiClient").Get<HttpClientSettings>(),
            jsonSerializerOptions, HttpPolicyKey.Retry, HttpPolicyKey.CircuitBreaker
        );

        services.AddHostedService<Worker>();
    });

builder.Build().Run();