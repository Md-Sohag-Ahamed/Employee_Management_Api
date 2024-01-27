using EmployeeManagement_WepApi.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Reflection.Metadata;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace EmployeeManagement_WepApi.Repository
{
    public class PdfService : IPdfService
    {
        //public byte[] GenerateCertificatePdf(CertificateModel certificate)
        //{
        //    using (var document = new PdfDocument())
        //    {
        //        var page = document.AddPage();
        //        using (var gfx = XGraphics.FromPdfPage(page))
        //        {
        //            // Customize the font, size, and formatting as needed
        //            var font = new XFont("Arial", 12);

        //            // Position and format the text on the PDF page
        //            var content = $"Certificate ID: {certificate.Id}\nEmployee ID: {certificate.EmployeeId}\nDate: {certificate.Date}";

        //            // Adjust the X, Y coordinates as needed
        //            gfx.DrawString(content, font, XBrushes.Black, new XRect(10, 10, page.Width, page.Height), XStringFormats.TopLeft);
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            document.Save(stream, false);
        //            return stream.ToArray();
        //        }
        //    }
        //}

        public byte[] GenerateCertificatePdf(EmployeeModel employee, CertificateModel certificate)
        {

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                // Add employee information
                document.Add(new Paragraph($"Employee Name: {employee.Name}"));
                document.Add(new Paragraph($"Employee Department: {employee.Department}"));

                // Add certificate information
                document.Add(new Paragraph($"Certificate ID: {certificate.Id}"));
                document.Add(new Paragraph($"Certificate Date: {certificate.Date}"));

                document.Close();
                return stream.ToArray();
            }
        }
    }
}
