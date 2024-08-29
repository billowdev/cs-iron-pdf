// PdfConversionService.cs
using IronPdf;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonPDFServices.Core.Ports;
using CommonPDFServices.Packages.Configs;

using System.Text.Json;
namespace CommonPDFServices.Core.Services
{
    public class PdfConversionService : IPdfConversionService
    {
        private readonly ILogger<PdfConversionService> _logger;
      private readonly IronPdfSettings _ironPdfSettings;

  public PdfConversionService(ILogger<PdfConversionService> logger, IronPdfSettings ironPdfSettings)
        {
            _logger = logger;
            _ironPdfSettings = ironPdfSettings;
        }

        public async Task<string> ConvertHtmlToPdfAsync(string htmlTemplate, Dictionary<string, string> content)
        {
                IronPdf.License.LicenseKey = _ironPdfSettings.LicenseKey;
            var htmlContent = htmlTemplate;
       
        //    string json = JsonConvert.SerializeObject(content, Formatting.Indented);
    foreach (var kvp in content)
    {
        // Replace placeholders in the template with actual values
        htmlContent = htmlContent.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
    }

            try

            {
                var renderer = new HtmlToPdf();
                var pdfDocument = renderer.RenderHtmlAsPdf(htmlContent);


string watermarkHtml = @"
<img src='https://ironsoftware.com/img/products/ironpdf-logo-text-dotnet.svg'>
<h1>Iron Software</h1>";

                // pdfDocument.ApplyWatermark(watermarkHtml, rotation: 45, opacity: 20);
                pdfDocument.ApplyWatermark(watermarkHtml, opacity: 20);


               var filePath = Path.Combine(Path.GetTempPath(), "health_certificates.pdf");
                pdfDocument.SaveAs(filePath);


                _logger.LogInformation("PDF saved to {FilePath}", filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting HTML to PDF.");
                throw;
            }
         
        }
    }
}
