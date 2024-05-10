using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.IO.Font;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using DataAccessLayer.Models.Entities;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace AiLearner_API.Services
{
    public class PdfService
    {
        public byte[] GeneratePdf(Material material)
        {
            using MemoryStream stream = new();
            using PdfWriter writer = new(stream);
            using PdfDocument pdf = new(writer);
            using Document document = new(pdf);

            // Font definitions
            PdfFont helvetica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont helveticaBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            // Adding Header
            AddHeader(document, material.Topic!, helveticaBold);

            // Adding Content Section
            AddContentSection(document, material.Content!, helvetica, helveticaBold);

            // Adding Questions on new page
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            AddQuestions(document, [..material.Questions], helvetica, helveticaBold);

            document.Close();
            return stream.ToArray();
        }

        private static void AddHeader(Document document, string headerText, PdfFont font)
        {
            Paragraph header = new Paragraph(headerText)
                .SetFont(font)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(header);
            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));
        }

        private static void AddContentSection(Document document, string content, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Content:")
                .SetFont(boldFont)
                .SetFontSize(12));
            document.Add(new Paragraph(content)
                .SetFont(normalFont)
                .SetFontSize(12));
        }

        private static void AddQuestions(Document document, List<Question> questions, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Questions")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));
            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));

            bool isFirstQuestion = true;
            foreach (var question in questions)
            {
                Paragraph questionParagraph = new Paragraph(question.Text)
                    .SetFont(boldFont)
                    .SetFontSize(12);

                if (!isFirstQuestion)
                {
                    questionParagraph.SetMarginTop(10);  // Space before each question
                }
                else
                {
                    isFirstQuestion = false;
                }

                document.Add(questionParagraph);

                List list = new List()
                    .SetSymbolIndent(12)
                    .SetListSymbol("\u2022")
                    .SetFont(normalFont);
                foreach (var answer in question.Answers!)
                {
                    list.Add(new ListItem(answer.Text));
                }
                document.Add(list);
            }
        }
    }

}
