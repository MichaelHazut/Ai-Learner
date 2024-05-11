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
using DataAccessLayer.Models;
using iText.Kernel.Events;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
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

            // Font definitions
            PdfFont helvetica = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont helveticaBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new CustomEventHandler());

            // Start a new document instance for the header and potentially other initial content
            Document headerDocument = new Document(pdf);
            AddHeader(headerDocument, material.Topic!, helveticaBold);
            headerDocument.Close();  // Close the header document

            // Adding content section
            Document contentDocument = new Document(pdf, pdf.GetDefaultPageSize());
            AddContentSection(contentDocument, material.Content!, helvetica, helveticaBold);
            contentDocument.Close(); // Ensure to close the document instance after finishing the content section

            // Adding questions section
            Document questionsDocument = new Document(pdf, pdf.GetDefaultPageSize());
            AddQuestions(questionsDocument, [..material.Questions], helvetica, helveticaBold);
            questionsDocument.Close(); // Close the document for questions section

            // Adding answers section
            Document answersDocument = new Document(pdf, pdf.GetDefaultPageSize());
            AddAnswers(answersDocument, [.. material.Questions], helvetica, helveticaBold);
            answersDocument.Close(); // Close the document for answers section

            // Revisit first page for Table of Contents
            Document tocDocument = new Document(pdf, pdf.GetDefaultPageSize());
            AddTableOfContent(tocDocument, helvetica, helveticaBold, material.Summery!, 2, pdf.GetNumberOfPages() - 1, pdf.GetNumberOfPages());
            tocDocument.Close(); // Close the document for the table of contents

            return stream.ToArray();
        }

        private static void AddHeader(Document document, string headerText, PdfFont font)
        {
            Paragraph header = new Paragraph(headerText)
                .SetFont(font)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER);
            document.Add(header);
            document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine(1)));
        }

        private static void AddTableOfContent(Document document, PdfFont normalFont, PdfFont boldFont, string summary, int contPages, int quesPages, int answPages)
        {
            document.Add(new Paragraph("Table of Contents")
                .SetFont(boldFont)
                .SetUnderline()
                .SetFontSize(16));

            document.Add(new Paragraph($"Summary Page: 1")
                .SetFont(boldFont)
                .SetFontSize(14))
                .SetTopMargin(16);

            document.Add(new Paragraph($"Content Pages: 2-{contPages - 2}")
                .SetFont(boldFont)
                .SetFontSize(14))
                .SetTopMargin(16);

            document.Add(new Paragraph($"Questions Pages: {contPages + 1}-{quesPages}")
                .SetFont(boldFont)
                .SetFontSize(14))
                .SetTopMargin(16);

            string answerPages = $"Answers Page: {answPages}";
            if (quesPages + 1 != answPages )
            {
                answerPages = $"Answers Pages: {quesPages + 1}-{answPages} ";
            }
            document.Add(new Paragraph(answerPages)
                .SetFont(boldFont)
                .SetFontSize(14))
                .SetTopMargin(16);

            document.Add(new Paragraph("Summary")
                .SetFont(boldFont)
                .SetFontSize(16)
                .SetTextAlignment(TextAlignment.CENTER))
                .SetTopMargin(28);
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
                .SetFontSize(14));
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
                    .SetSymbolIndent(12)
                    .SetFont(normalFont);

                int index = 1;
                foreach (var answer in question.Answers!)
                {
                    list.Add(new ListItem($"{index}) {answer.Text}"));
                    index++;
                }
                document.Add(list);
            }
        }
        private static void AddAnswers(Document document, List<Question> questions, PdfFont normalFont, PdfFont boldFont)
        {
            document.Add(new Paragraph("Answers")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));
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
