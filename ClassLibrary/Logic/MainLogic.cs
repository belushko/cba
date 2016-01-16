using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace cba.Logic
{
    public class MainLogic
    {
        public void FillNumbers(Series[] series, int size, int sizeOfRows)
        {
            var random = new Random(unchecked((int) DateTime.Now.Ticks));

            for (var k = 0; k < size; k++)
            {
                for (var i = 0; i < sizeOfRows; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        series[k].a[i][j] = random.Next(1, 2);

                        if ((j + 1)%3 == 0)
                        {
                            series[k].a[i][j] = 0; //i row j column
                        }
                    }
                }
            }
        }

        public int Culculate(Series[] series, DataArray[] a, int moValue, bool first, int size)
        {
            var solver = new Solver();
            first = false;

            for (var i = 0; i < 3; i++)
            {
                series[i].MOValue = moValue;
            }

            var isTableValid = true;

            for (var k = 0; k < 3; k++)
            {
                //change series[0].Size
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        //if (series[k].a[i][j] < 0)
                        //{
                        //    isTableValid = false;
                        //}
                    }
                }
            }

            if (isTableValid)
            {
                for (var i = 0; i < 3; i++)
                {
                    series[i].FindMidValues();
                    series[i].FindBreedingSum();
                    series[i].FindActivityValue();
                }

                for (var i = 0; i < 3; i++)
                {
                    a[i] = series[i].a;
                }

                //TODO: refartor
                solver.Analise(size, series);

                return solver.bestMedNum + 1;
            }

            throw new Exception("Не все препараты прошли проверку!");
        }

        public void Save(Series[] series, DataArray[] a, int size, int sizeOfSeries)
        {
            var access = new Access();

            for (var k = 0; k < 3; k++)
            {
                series[k].SizeOfSeries = sizeOfSeries;
                series[k].Size = size;
                series[k].Mas.Clear();

                for (var i = 0; i < size; i++)
                {
                    var mas = new double[12];

                    for (var j = 0; j < 12; j++)
                    {
                        mas[j] = a[k][i][j];
                    }

                    series[k].Mas.Add(mas);
                }
            }

            access.Save("Info.xml", series);
        }

        public void Load(Series[] series, DataArray[] a)
        {
            for (var k = 0; k < 3; k++)
            {
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        a[k][i][j] = series[k].Mas[i][j];
                    }
                }
            }
        }

        public void MakeReport(string fileName, string nameA, string nameB, string nameC, Series[] series, DataArray[] a,
            int numberOfSeries)
        {
            var doc = new Document(PageSize.LETTER, 10, 10, 42, 35);

            var pdfWriter = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
            doc.Open();

            //шрифты
            var baseFont = BaseFont.CreateFont(@"arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

            var seriesName1 = "\n" + nameA + "\n ";
            var paragraph1 = new Paragraph(seriesName1, new Font(baseFont, 14, Font.NORMAL));
            paragraph1.Alignment = 1;
            doc.Add(paragraph1);

            //series 1
            var table1 = new PdfPTable(5);

            var cell11 =
                new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)",
                    new Font(baseFont, 14, Font.NORMAL)));
            cell11.Colspan = 5;
            cell11.HorizontalAlignment = 1;
            table1.AddCell(cell11);

            var cell12 = new PdfPCell(new Phrase(""));
            cell12.Colspan = 1;
            cell12.HorizontalAlignment = 1;
            table1.AddCell(cell12);

            var cell13 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
            cell13.Colspan = 2;
            cell13.HorizontalAlignment = 1;
            table1.AddCell(cell13);

            var cell14 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
            cell14.Colspan = 2;
            cell14.HorizontalAlignment = 1;
            table1.AddCell(cell14);

            var cell15 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell15);
            var cell16 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell16);
            var cell17 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell17);
            var cell18 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell18);
            var cell19 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell19);

            for (var i = 0; i < a[0].M; i++)
            {
                table1.AddCell(i + 1 + "");
                table1.AddCell(series[0].a[i][2] + " (" + series[0].a[i][0] + " x " + series[0].a[i][1] + ")");
                table1.AddCell(series[0].a[i][5] + " (" + series[0].a[i][3] + " x " + series[0].a[i][4] + ")");
                table1.AddCell(series[0].a[i][8] + " (" + series[0].a[i][6] + " x " + series[0].a[i][7] + ")");
                table1.AddCell(series[0].a[i][11] + " (" + series[0].a[i][9] + " x " + series[0].a[i][10] + ")");
            }

            doc.Add(table1);
            var text1 = "\nАктивність = " + Math.Round(series[0].activity) + " МО";
            var paragraph12 = new Paragraph(text1, new Font(baseFont, 14, Font.NORMAL));
            paragraph12.Alignment = 1;
            doc.Add(paragraph12);

            //series 2
            if ((numberOfSeries == 2) ||
                (numberOfSeries == 3))
            {
                var seriesName2 = "\n" + nameB + "\n ";
                var paragraph2 = new Paragraph(seriesName2, new Font(baseFont, 14, Font.NORMAL));
                paragraph2.Alignment = 1;
                doc.Add(paragraph2);

                var table2 = new PdfPTable(5);

                var cell21 =
                    new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)",
                        new Font(baseFont, 14, Font.NORMAL)));
                cell21.Colspan = 5;
                cell21.HorizontalAlignment = 1;
                table2.AddCell(cell21);

                var cell22 = new PdfPCell(new Phrase(""));
                cell22.Colspan = 1;
                cell22.HorizontalAlignment = 1;
                table2.AddCell(cell22);

                var cell23 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell23.Colspan = 2;
                cell23.HorizontalAlignment = 1;
                table2.AddCell(cell23);

                var cell24 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell24.Colspan = 2;
                cell24.HorizontalAlignment = 1;
                table2.AddCell(cell24);

                var cell25 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell25);
                var cell26 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell26);
                var cell27 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell27);
                var cell28 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell28);
                var cell29 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell29);

                for (var i = 0; i < a[1].M; i++)
                {
                    table2.AddCell(i + 1 + "");
                    table2.AddCell(series[1].a[i][2] + " (" + series[1].a[i][0] + " x " + series[1].a[i][1] + ")");
                    table2.AddCell(series[1].a[i][5] + " (" + series[1].a[i][3] + " x " + series[1].a[i][4] + ")");
                    table2.AddCell(series[1].a[i][8] + " (" + series[1].a[i][6] + " x " + series[1].a[i][7] + ")");
                    table2.AddCell(series[1].a[i][11] + " (" + series[1].a[i][9] + " x " + series[1].a[i][10] + ")");
                }

                doc.Add(table2);

                var text2 = "\nАктивність = " + Math.Round(series[1].activity) + " MO";
                var paragraph22 = new Paragraph(text2, new Font(baseFont, 14, Font.NORMAL));
                paragraph22.Alignment = 1;
                doc.Add(paragraph22);
            }

            //series 3
            if (numberOfSeries == 3)
            {
                var seriesName3 = "\n" + nameC + "\n ";
                var paragraph3 = new Paragraph(seriesName3, new Font(baseFont, 14, Font.NORMAL));
                paragraph3.Alignment = 1;
                doc.Add(paragraph3);

                var table3 = new PdfPTable(5);

                var cell31 =
                    new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)",
                        new Font(baseFont, 14, Font.NORMAL)));
                cell31.Colspan = 5;
                cell31.HorizontalAlignment = 1;
                table3.AddCell(cell31);

                var cell32 = new PdfPCell(new Phrase(""));
                cell32.Colspan = 1;
                cell32.HorizontalAlignment = 1;
                table3.AddCell(cell32);

                var cell33 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell33.Colspan = 2;
                cell33.HorizontalAlignment = 1;
                table3.AddCell(cell33);

                var cell34 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell34.Colspan = 2;
                cell34.HorizontalAlignment = 1;
                table3.AddCell(cell34);

                var cell35 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell35);
                var cell36 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell36);
                var cell37 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell37);
                var cell38 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell38);
                var cell39 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell39);

                for (var i = 0; i < a[1].M; i++)
                {
                    table3.AddCell(i + 1 + "");
                    table3.AddCell(series[1].a[i][2] + " (" + series[1].a[i][0] + " x " + series[1].a[i][1] + ")");
                    table3.AddCell(series[1].a[i][5] + " (" + series[1].a[i][3] + " x " + series[1].a[i][4] + ")");
                    table3.AddCell(series[1].a[i][8] + " (" + series[1].a[i][6] + " x " + series[1].a[i][7] + ")");
                    table3.AddCell(series[1].a[i][11] + " (" + series[1].a[i][9] + " x " + series[1].a[i][10] + ")");
                }

                doc.Add(table3);

                var text3 = "\nАктивність = " + Math.Round(series[2].activity) + " MO";
                var paragraph32 = new Paragraph(text3, new Font(baseFont, 14, Font.NORMAL));
                paragraph32.Alignment = 1;
                doc.Add(paragraph32);

                var text4 = "\n                         Підпис:";
                var paragraph4 = new Paragraph(text4, new Font(baseFont, 14, Font.NORMAL));
                paragraph4.Alignment = 0;
                doc.Add(paragraph4);
            }

            doc.Close();
        }
    }
}