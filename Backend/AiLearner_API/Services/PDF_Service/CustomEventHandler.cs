using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Kernel.Events;


namespace AiLearner_API.Services.PDF_Service
{
    public class CustomEventHandler : IEventHandler
    {

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent documentEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = documentEvent.GetDocument();
            PdfPage page = documentEvent.GetPage();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            Rectangle pageSizeRect = page.GetPageSize();  // This is the Rectangle object

            // Creating a PageSize from Rectangle
            PageSize pageSize = new(pageSizeRect);  // Correct usage

            // Example of adding a footer
            float footerY = 30;

            pdfCanvas.MoveTo(30, footerY)
                     .LineTo(pageSize.GetWidth() - 30, footerY)
                     .SetLineWidth(1)
                     .Stroke();

            pdfCanvas.BeginText()
                     .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                     .MoveText(30, footerY - 15)
                     .ShowText("© 2024 AiLearner.online, All rights reserved.")
                     .EndText();

            pdfCanvas.BeginText()
                     .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                     .MoveText(pageSize.GetWidth() - 70, footerY - 15)
                     .ShowText($"Page {pdfDoc.GetPageNumber(page)}")
                     .EndText();

            pdfCanvas.Release();
        }
    }

}
