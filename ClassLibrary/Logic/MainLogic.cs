using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace cba.Logic
{
    public class MainLogic
    {
        public void FillNumbers(DataArray[] arrays, int size)
        {
            Random random = new Random(unchecked((int) (DateTime.Now.Ticks)));

            for (int k = 0; k < size; k++)
            {
                for (int i = 0; i < arrays[k].M; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        arrays[k][i][j] = random.Next(5, 12);

                        if ((j + 1)%3 == 0)
                        {
                            arrays[k][i][j] = 0; //i row j column
                        }
                    }
                }
            }
        }

        public int Culculate(Series[] series, DataArray[] a, int moValue, bool first, int size)
        {
            Solver solver = new Solver();
            first = false;

            for (int i = 0; i < 3; i++)
            {
                series[i].a = a[i];
                series[i].MOValue = moValue;
            }

            bool isTableValid = true;

            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < a[k].M; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (a[k][i][j] < 0)
                        {
                            isTableValid = false;
                        }
                    }
                }
            }

            if (isTableValid)
            {
                for (int i = 0; i < 3; i++)
                {
                    series[i].FindMidValues();
                    series[i].FindBreedingSum();
                    series[i].FindActivityValue();
                }

                for (int i = 0; i < 3; i++)
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
            Access access = new Access();

            for (int k = 0; k < 3; k++)
            {
                series[k].SizeOfSeries = sizeOfSeries;
                series[k].Size = size;
                series[k].Mas.Clear();

                for (int i = 0; i < size; i++)
                {
                    double[] mas = new double[12];

                    for (int j = 0; j < 12; j++)
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

            for (int k = 0; k < 3; k++)
            {

                for (int i = 0; i < series[0].Size; i++)
                {

                    for (int j = 0; j < 12; j++)
                    {
                        a[k][i][j] = series[k].Mas[i][j];
                    }
                }
            }
        }

        public void MakeReport(string fileName, string nameA, string nameB, string nameC, Series[] series, DataArray[] a, int numberOfSeries)
        {
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);

            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));
            doc.Open();

            //шрифты
            BaseFont baseFont = BaseFont.CreateFont(@"arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            string seriesName1 = "\n" + nameA + "\n ";
            iTextSharp.text.Paragraph paragraph1 = new iTextSharp.text.Paragraph(seriesName1, new Font(baseFont, 14, Font.NORMAL));
            paragraph1.Alignment = 1;
            doc.Add(paragraph1);

            //series 1
            PdfPTable table1 = new PdfPTable(5);

            PdfPCell cell11 = new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)", new Font(baseFont, 14, Font.NORMAL)));
            cell11.Colspan = 5;
            cell11.HorizontalAlignment = 1;
            table1.AddCell(cell11);

            PdfPCell cell12 = new PdfPCell(new Phrase(""));
            cell12.Colspan = 1;
            cell12.HorizontalAlignment = 1;
            table1.AddCell(cell12);

            PdfPCell cell13 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
            cell13.Colspan = 2;
            cell13.HorizontalAlignment = 1;
            table1.AddCell(cell13);

            PdfPCell cell14 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
            cell14.Colspan = 2;
            cell14.HorizontalAlignment = 1;
            table1.AddCell(cell14);

            PdfPCell cell15 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell15);
            PdfPCell cell16 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell16);
            PdfPCell cell17 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell17);
            PdfPCell cell18 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell18);
            PdfPCell cell19 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
            table1.AddCell(cell19);

            for (int i = 0; i < a[0].M; i++)
            {
                table1.AddCell((i + 1) + "");
                table1.AddCell(series[0].a[i][2] + " (" + series[0].a[i][0] + " x " + series[0].a[i][1] + ")");
                table1.AddCell(series[0].a[i][5] + " (" + series[0].a[i][3] + " x " + series[0].a[i][4] + ")");
                table1.AddCell(series[0].a[i][8] + " (" + series[0].a[i][6] + " x " + series[0].a[i][7] + ")");
                table1.AddCell(series[0].a[i][11] + " (" + series[0].a[i][9] + " x " + series[0].a[i][10] + ")");
            }

            doc.Add(table1);
            string text1 = "\nАктивність = " + Math.Round(series[0].activity) + " МО";
            iTextSharp.text.Paragraph paragraph12 = new iTextSharp.text.Paragraph(text1, new Font(baseFont, 14, Font.NORMAL));
            paragraph12.Alignment = 1;
            doc.Add(paragraph12);

            //series 2
            if ((numberOfSeries == 2) ||
                (numberOfSeries == 3))
            {
                string seriesName2 = "\n" + nameB + "\n ";
                iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph(seriesName2, new Font(baseFont, 14, Font.NORMAL));
                paragraph2.Alignment = 1;
                doc.Add(paragraph2);

                PdfPTable table2 = new PdfPTable(5);

                PdfPCell cell21 = new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)", new Font(baseFont, 14, Font.NORMAL)));
                cell21.Colspan = 5;
                cell21.HorizontalAlignment = 1;
                table2.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(""));
                cell22.Colspan = 1;
                cell22.HorizontalAlignment = 1;
                table2.AddCell(cell22);

                PdfPCell cell23 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell23.Colspan = 2;
                cell23.HorizontalAlignment = 1;
                table2.AddCell(cell23);

                PdfPCell cell24 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell24.Colspan = 2;
                cell24.HorizontalAlignment = 1;
                table2.AddCell(cell24);

                PdfPCell cell25 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell25);
                PdfPCell cell26 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell26);
                PdfPCell cell27 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell27);
                PdfPCell cell28 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell28);
                PdfPCell cell29 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
                table2.AddCell(cell29);

                for (int i = 0; i < a[1].M; i++)
                {
                    table2.AddCell((i + 1) + "");
                    table2.AddCell(series[1].a[i][2] + " (" + series[1].a[i][0] + " x " + series[1].a[i][1] + ")");
                    table2.AddCell(series[1].a[i][5] + " (" + series[1].a[i][3] + " x " + series[1].a[i][4] + ")");
                    table2.AddCell(series[1].a[i][8] + " (" + series[1].a[i][6] + " x " + series[1].a[i][7] + ")");
                    table2.AddCell(series[1].a[i][11] + " (" + series[1].a[i][9] + " x " + series[1].a[i][10] + ")");
                }

                doc.Add(table2);

                string text2 = "\nАктивність = " + Math.Round(series[1].activity) + " MO";
                iTextSharp.text.Paragraph paragraph22 = new iTextSharp.text.Paragraph(text2, new Font(baseFont, 14, Font.NORMAL));
                paragraph22.Alignment = 1;
                doc.Add(paragraph22);
            }

            //series 3
            if ((numberOfSeries == 3))
            {
                string seriesName3 = "\n" + nameC + "\n ";
                iTextSharp.text.Paragraph paragraph3 = new iTextSharp.text.Paragraph(seriesName3, new Font(baseFont, 14, Font.NORMAL));
                paragraph3.Alignment = 1;
                doc.Add(paragraph3);

                PdfPTable table3 = new PdfPTable(5);

                PdfPCell cell31 = new PdfPCell(new Phrase("Інтенсивність реакції на введення розведень туберкуліну (мм)", new Font(baseFont, 14, Font.NORMAL)));
                cell31.Colspan = 5;
                cell31.HorizontalAlignment = 1;
                table3.AddCell(cell31);

                PdfPCell cell32 = new PdfPCell(new Phrase(""));
                cell32.Colspan = 1;
                cell32.HorizontalAlignment = 1;
                table3.AddCell(cell32);

                PdfPCell cell33 = new PdfPCell(new Phrase("Дослідна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell33.Colspan = 2;
                cell33.HorizontalAlignment = 1;
                table3.AddCell(cell33);

                PdfPCell cell34 = new PdfPCell(new Phrase("Контрольна серія", new Font(baseFont, 14, Font.NORMAL)));
                cell34.Colspan = 2;
                cell34.HorizontalAlignment = 1;
                table3.AddCell(cell34);

                PdfPCell cell35 = new PdfPCell(new Phrase("Умовний номер", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell35);
                PdfPCell cell36 = new PdfPCell(new Phrase("Розведення I", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell36);
                PdfPCell cell37 = new PdfPCell(new Phrase("Розведення II", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell37);
                PdfPCell cell38 = new PdfPCell(new Phrase("Розведення III", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell38);
                PdfPCell cell39 = new PdfPCell(new Phrase("Розведення IV", new Font(baseFont, 12, Font.NORMAL)));
                table3.AddCell(cell39);

                for (int i = 0; i < a[1].M; i++)
                {
                    table3.AddCell((i + 1) + "");
                    table3.AddCell(series[1].a[i][2] + " (" + series[1].a[i][0] + " x " + series[1].a[i][1] + ")");
                    table3.AddCell(series[1].a[i][5] + " (" + series[1].a[i][3] + " x " + series[1].a[i][4] + ")");
                    table3.AddCell(series[1].a[i][8] + " (" + series[1].a[i][6] + " x " + series[1].a[i][7] + ")");
                    table3.AddCell(series[1].a[i][11] + " (" + series[1].a[i][9] + " x " + series[1].a[i][10] + ")");
                }

                doc.Add(table3);

                string text3 = "\nАктивність = " + Math.Round(series[2].activity) + " MO";
                iTextSharp.text.Paragraph paragraph32 = new iTextSharp.text.Paragraph(text3, new Font(baseFont, 14, Font.NORMAL));
                paragraph32.Alignment = 1;
                doc.Add(paragraph32);

                string text4 = "\n                         Підпис:";
                iTextSharp.text.Paragraph paragraph4 = new iTextSharp.text.Paragraph(text4, new Font(baseFont, 14, Font.NORMAL));
                paragraph4.Alignment = 0;
                doc.Add(paragraph4);
            }

            doc.Close();
        }
    }
}
