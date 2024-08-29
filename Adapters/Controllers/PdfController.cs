// Controllers/PdfController.cs
using Microsoft.AspNetCore.Mvc;
using CommonPDFServices.Core.Services;
using CommonPDFServices.Core.Domain;
using System.Threading.Tasks;

namespace CommonPDFServices.Adapters.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly PdfConversionService _pdfService;

        public PdfController(PdfConversionService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertHtmlToPdf([FromBody] PdfRequest request)
        {
            try
            {
                var filePath = await _pdfService.ConvertHtmlToPdfAsync(request.HtmlTemplate, request.Content);
                return PhysicalFile(filePath, "application/pdf", "converted.pdf");
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during PDF conversion.");
            }
        }
    }
}
