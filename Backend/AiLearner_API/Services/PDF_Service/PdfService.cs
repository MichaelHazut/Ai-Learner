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
using iText.Kernel.Events;
using iText.Layout.Renderer;


namespace AiLearner_API.Services.PDF_Service
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

            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new CustomEventHandler());

            // Adding Header
            AddHeader(document, material.Topic!, helveticaBold);

            AddSummary(document, material.Summery!, helvetica, helveticaBold);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            // Adding Content Section
            AddContentSection(document, material.Content!, helvetica, helveticaBold);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            // Adding Questions on new page
            AddQuestions(document, [.. material.Questions], helvetica, helveticaBold);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            AddAnswers(document, [.. material.Questions], helvetica, helveticaBold);

            document.Close();
            return stream.ToArray();
        }


        private static void AddHeader(Document document, string headerText, PdfFont font)
        {
            Paragraph header = new Paragraph(headerText)
                .SetFont(font)
                .SetFontSize(50)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetUnderline();
            document.Add(header);
        }
        private static void AddSummary(Document document, string summary, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Summary")
               .SetFont(boldFont)
               .SetFontSize(22)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetMarginTop(60));

            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));

            document.Add(new Paragraph(summary)
                .SetFont(normalFont)
                .SetFontSize(14))
                .SetTopMargin(20);
        }


        private static void AddContentSection(Document document, string content, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Content:")
                .SetFont(boldFont)
                .SetFontSize(22)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(12));

            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));

            document.Add(new Paragraph(content)
                .SetFont(normalFont)
                .SetFontSize(12));
        }

        private static void AddQuestions(Document document, List<Question> questions, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Questions")
                .SetFont(boldFont)
                .SetFontSize(22)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(12));
            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));
            int questionIndex = 0;
            foreach (var question in questions)
            {
                questionIndex++;
                Paragraph questionParagraph = new Paragraph($"{questionIndex}) {question.Text}")
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetMarginTop(16);

                document.Add(questionParagraph);

                List list = new List()
                    .SetListSymbol(ListNumberingType.DECIMAL)
                    .SetSymbolIndent(8)
                    .SetFont(normalFont);

                foreach (var answer in question.Answers!)
                {
                    list.Add(new ListItem(answer.Text));
                }
                document.Add(list);
            }
        }
        private static void AddAnswers(Document document, List<Question> questions, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Answers")
                .SetFont(boldFont)
                .SetFontSize(22)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(12));
            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));

            int questionIndex = 0;
            foreach (var question in questions)
            {
                questionIndex++;
                Paragraph answersParagraph = new Paragraph($"{questionIndex}) {question.Text}")
                    .SetFont(boldFont)
                    .SetFontSize(12)
                    .SetMarginTop(12);

                document.Add(answersParagraph);

                var correctAnswer = question.Answers!.FirstOrDefault(answer => answer.IsCorrect);
                if (correctAnswer == null)
                {
                    continue;
                }
                int answerIndex = question.Answers!.ToList().IndexOf(correctAnswer) + 1;

                document.Add(new Paragraph($"{answerIndex}) {correctAnswer.Text}")
                    .SetFont(normalFont)
                    .SetMarginLeft(12));

            }
        }
    }
}
