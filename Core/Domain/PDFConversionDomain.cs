

namespace CommonPDFServices.Core.Domain
{
   public record PdfRequest(string HtmlTemplate, Dictionary<string, string> Content);
}
