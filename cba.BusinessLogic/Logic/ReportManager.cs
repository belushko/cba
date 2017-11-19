using System;
using System.IO;
using CBA.BusinessLogic.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CBA.BusinessLogic.Logic
{
    public class ReportManager
    {
        public void MakeReport(string fileName, Series[] series, int numberOfSeries)
        {
            var document = new Document(PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
            document.Open();

            var baseFont = BaseFont.CreateFont(@"times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //series 1
            AddSeriesToPdfDocument(baseFont, series[0], document);

            //series 2
            if ((numberOfSeries == 2) || (numberOfSeries == 3))
            {
                AddSeriesToPdfDocument(baseFont, series[1], document);
            }

            //series 3
            if (numberOfSeries == 3)
            {
                AddSeriesToPdfDocument(baseFont, series[2], document);
            }

            document.Close();
        }

        private void AddSeriesToPdfDocument(BaseFont baseFont, Series series, Document document)
        {
            var finalSeriesName = "\n" + series.Name + "\n ";

            var headerParagraph = CreateParagraph(baseFont, finalSeriesName);

            var pdfTable = CreatePdfTable(baseFont);
            AddSeriesToPdfTable(series, pdfTable);

            var footerText = $"\nАктивність = {Math.Round(series.Activity)} МО";
            var footerParagraph = CreateParagraph(baseFont, footerText);

            document.Add(headerParagraph);
            document.Add(pdfTable);
            document.Add(footerParagraph);
        }

        private Paragraph CreateParagraph(BaseFont baseFont, string text)
        {
            var font14 = new Font(baseFont, 14, Font.NORMAL);

            var finalSeriesName = "\n" + text + "\n ";
            return new Paragraph(finalSeriesName, font14)
            {
                Alignment = 1
            };
        }
        
        private PdfPTable CreatePdfTable(BaseFont baseFont)
        {
            var font14 = new Font(baseFont, 14, Font.NORMAL);
            var font12 = new Font(baseFont, 12, Font.NORMAL);

            var pdfTable = new PdfPTable(5);

            var cell_1 =
                new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)", font14))
                {
                    Colspan = 5,
                    HorizontalAlignment = 1
                };

            pdfTable.AddCell(cell_1);

            var cell_2 = new PdfPCell(new Phrase(""))
            {
                Colspan = 1,
                HorizontalAlignment = 1
            };

            pdfTable.AddCell(cell_2);

            var cell_3 = new PdfPCell(new Phrase("Дослідна серія", font14))
            {
                Colspan = 2,
                HorizontalAlignment = 1
            };

            pdfTable.AddCell(cell_3);

            var cell_4 = new PdfPCell(new Phrase("Контрольна серія", font14))
            {
                Colspan = 2,
                HorizontalAlignment = 1
            };

            pdfTable.AddCell(cell_4);

            var cell_5 = new PdfPCell(new Phrase("Умовний номер", font12));
            pdfTable.AddCell(cell_5);

            var cell_6 = new PdfPCell(new Phrase("Розведення I", font12));
            pdfTable.AddCell(cell_6);

            var cell_7 = new PdfPCell(new Phrase("Розведення II", font12));
            pdfTable.AddCell(cell_7);

            var cell_8 = new PdfPCell(new Phrase("Розведення III", font12));
            pdfTable.AddCell(cell_8);

            var cell_9 = new PdfPCell(new Phrase("Розведення IV", font12));
            pdfTable.AddCell(cell_9);

            return pdfTable;
        }

        private void AddSeriesToPdfTable(Series series, PdfPTable pdfTable)
        {
            for (var i = 0; i < series.Size; i++)
            {
                var data = series.DataArray[i];

                pdfTable.AddCell($"{ i + 1 }");
                pdfTable.AddCell($"{ data[2] } ({ data[0] } x { data[1] })");
                pdfTable.AddCell($"{ data[5] } ({ data[3] } x { data[4] })");
                pdfTable.AddCell($"{ data[8] } ({ data[6] } x { data[7] })");
                pdfTable.AddCell($"{ data[11] } ({ data[9] } x { data[10] })");
            }
        }
    }
}
