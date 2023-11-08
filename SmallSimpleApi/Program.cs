using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SmallSimpleApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add versioning in the API controllers.
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Swagger with a security header integration
builder.Services.AddSwaggerGen(swaggerOptions =>
{
    swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Small Simple API", Version = "v1" });
    swaggerOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Small Simple API.xml"));
    swaggerOptions.AddSecurityDefinition(
        "X-SECURITY-HEADER",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = $"Your X-SECURITY-HEADER",
            Name = "X-SECURITY-HEADER",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "X-SECURITY-HEADER"
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
                        Id = "X-SECURITY-HEADER"
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

// Add our custom services
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