using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace OliverScheer.OpenApiSample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        // Add Swagger
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Oliver Scheer Sample API",
                Version = "v1",
                Description = "API to to demonstrate some features."
            });

            // Add JWT-Authentication in Swagger UI
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Geben Sie 'Bearer {Token}' ein, um sich zu authentifizieren."
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
                    Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Swagger Start
            // JSON: /swagger/v1/swagger.json
            // UI: /swagger
            app.UseSwagger();
            app.UseSwaggerUI();
            // Swagger End

            // Scalar Start
            // UI: /scalar/v1
            app.MapScalarApiReference();
            // JSON  --> /scalar/v1
            app.MapOpenApi();
            // Scalar end
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.MapGroup("/api/v1").MapGet("/helloworld", () => "Hello Hello!");
        app.MapGroup("/api/v1").MapGet("/time", () =>
        {
            return Results.Ok(
                new
                {
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
        });

        app.MapControllers();

        app.Run();
    }
}
