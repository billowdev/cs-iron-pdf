// Program.cs
using DotNetEnv;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommonPDFServices.Core.Services;
using CommonPDFServices.Core.Ports;
using CommonPDFServices.Core.Domain;
using CommonPDFServices.Packages.Configs;
using IronPdf;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure IronPDF settings from the environment variable
var ironPdfSettings = new IronPdfSettings
{
    LicenseKey = EnvironmentConfig.IronPdfLicenseKey
};

builder.Services.AddSingleton(ironPdfSettings); // Register IronPDF settings

builder.Services.AddScoped<IPdfConversionService, PdfConversionService>();
// builder.WebHost.ConfigureKestrel(serverOptions =>
// {
//     serverOptions.ListenAnyIP(5000); // Listen on HTTP port
// });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define the PDF conversion endpoint
app.MapPost("/api/pdf/convert", async (PdfRequest request, IPdfConversionService pdfService) =>
{
    try
    {
        // app.Logger.LogInformation("----------");
        // app.Logger.LogInformation(request.HtmlTemplate);
        // // app.Logger.LogInformation(request.Content);
        // app.Logger.LogInformation("----------");

        var pdfBytes = await pdfService.ConvertHtmlToPdfAsync(request.HtmlTemplate, request.Content);
        return Results.File(pdfBytes, "application/pdf", "converted.pdf");
        
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error occurred during PDF conversion.");
        return Results.Problem("An error occurred during PDF conversion.");
    }
})
.WithName("ConvertHtmlToPdf")
.WithOpenApi();


// Example weather forecast endpoint
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

var address = app.Urls.FirstOrDefault() ?? "http://localhost:5000";
app.Logger.LogInformation("Application is running on: {Address}", address);

app.Run();

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
