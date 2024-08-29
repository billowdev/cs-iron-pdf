// internal/core/ports/IPdfConversionService.cs
namespace CommonPDFServices.Core.Ports
{
    public interface IPdfConversionService
    {
       Task<string> ConvertHtmlToPdfAsync(string htmlTemplate,  Dictionary<string, string> content);
    }
}
