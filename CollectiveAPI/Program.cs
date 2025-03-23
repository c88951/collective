using CollectiveData.Extensions;
using Microsoft.EntityFrameworkCore;
using CollectiveData.Data;
using Microsoft.AspNetCore.Mvc;
using CollectiveData.Helpers;
using System.Text.Json.Serialization;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Collective API");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Preserve object references to handle circular references
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            // Use camelCase for property names
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        });

    // Configure API behavior options for standardized responses
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value?.Errors ?? new())
                .Select(x => x.ErrorMessage)
                .ToList();

            var result = ApiResponse<object>.BadRequest("Validation failed", errors);
            return new BadRequestObjectResult(result);
        };
    });

    // Add DbContext
    builder.Services.AddDbContext<CollectiveDbContext>(options =>
        options.UseInMemoryDatabase("CollectiveDb"));

    // Add repositories
    builder.Services.AddCollectiveDataServices();

    // Add health checks
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Collective API", Version = "v1" });
    });

    // Add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Collective API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });

    // Add health check endpoint
    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

    // Add Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    // Disable HTTPS redirection for non-production environment
    if (!app.Environment.IsProduction())
    {
        app.Use(async (context, next) =>
        {
            // Log all requests
            Log.Information("Request: {Method} {Path}", context.Request.Method, context.Request.Path);
            await next();
            // Log response status
            Log.Information("Response: {StatusCode} for {Method} {Path}", 
                context.Response.StatusCode, context.Request.Method, context.Request.Path);
        });
    }
    else
    {
        app.UseHttpsRedirection();
    }

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    // Seed the database
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        DataSeeder.Initialize(services);
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
