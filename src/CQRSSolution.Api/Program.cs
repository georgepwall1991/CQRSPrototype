using CQRSSolution.Application;
using CQRSSolution.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi; // Assuming OpenApiInfo is here or available via implicit using from Swashbuckle?
using System.Reflection;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddMvc() // Add MVC support
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());

// Configure OpenTelemetry Logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
    options.AddOtlpExporter();
});

// Configure Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // In a real scenario, these would be loaded from Configuration
        options.Authority = builder.Configuration["Authentication:Authority"] ?? "https://demo.duendesoftware.com";
        options.Audience = builder.Configuration["Authentication:Audience"] ?? "api";
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
    });

// Register Application Layer services
builder.Services.AddApplicationServices();

// Register Infrastructure Layer services
builder.Services.AddInfrastructureServices(builder.Configuration);

// API Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    /* 
    // TODO: Fix namespace resolution for OpenApiSecurityScheme (Microsoft.OpenApi.Models issue)
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CQRSSolution API",
        Description = "API for managing orders using CQRS and Outbox Pattern."
    });

    // Add JWT Bearer definition to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    */
    
    // Minimal Swagger doc
    options.SwaggerDoc("v1", new OpenApiInfo // Trying full qualification? Or just OpenApiInfo
    {
        Version = "v1",
        Title = "CQRSSolution API",
        Description = "API for managing orders using CQRS and Outbox Pattern."
    });

    // Optionally, include XML comments for Swagger UI if set up in .csproj
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CQRSSolution API V1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at app root
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Enable Authentication Middleware

app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();

public partial class Program { }