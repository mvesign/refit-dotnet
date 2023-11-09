using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SharedModels;
using SmallSimpleApi.Services;
using SmallSimpleApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add versioning in the API controllers.
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add Swagger with a security header integration
builder.Services.AddSwaggerGen(swaggerOptions =>
{
    swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Small Simple API", Version = "1.0" });
    swaggerOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SmallSimpleAPI.xml"));
    swaggerOptions.AddSecurityDefinition(
        ApiHeaderSettings.ApiHeaderKey,
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = $"Your {ApiHeaderSettings.ApiHeaderKey}",
            Name = ApiHeaderSettings.ApiHeaderKey,
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiHeaderSettings.ApiHeaderKey
        }
    );
    swaggerOptions.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiHeaderSettings.ApiHeaderKey
                    },
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        }
    );
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add our custom services and settings
builder.Services.Configure<AccountSettings>(builder.Configuration.GetSection("HttpPolicies"));
builder.Services.AddSingleton<AccountsService>();

// And finally add the API controllers.
builder.Services.AddControllers();

var app = builder.Build();

// Now we need to enable Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Map the controllers.
app.MapControllers();

// And run the application.
app.Run();