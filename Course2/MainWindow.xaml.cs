using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.IO;
using cba;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading;
using cba.Logic;

namespace cba
{
    public partial class MainWindow : Window
    {
        InfoWindow info = new InfoWindow();
        DataArray[] a = new DataArray[3];
        Series[] series = new Series[3];
        Solver solver = new Solver();
        Access access = new Access();
        bool first = true;

        private MainLogic logic = new MainLogic();

        public MainWindow()
        {
            info.Close();
            InitializeComponent();

            for (int i = 2; i <= 15; i++)
                comboBoxN.Items.Add(i);
            comboBoxN.SelectedItem = 5;

            for (int i = 1; i <= 3; i++)
                comboBoxNumberOfSeries.Items.Add(i);
            comboBoxNumberOfSeries.SelectedItem = 3;

            int size = int.Parse(comboBoxN.SelectedValue + "");

            for (int i = 0; i < 3; i++)
            {
                series[i] = new Series(size);
                series[i].a = new DataArray(size, size);
                series[i].Size = size;
                series[i].MOValue = int.Parse(textBoxMO.Text);//!!!
                series[i].InitTable();
            }

            series[0].Name = "A";
            series[1].Name = "B";
            series[2].Name = "C";

            InitTable(size);
        }

        private void buttonFill_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");

            logic.FillNumbers(a, size);
        }

        private void buttonCulc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");
                int moValue = int.Parse(textBoxMO.Text);

                int best = logic.Culculate(series, a, moValue, first, size);

                string bestMedNumber = "";

                switch (best)
                {
                    case 1:
                        bestMedNumber = tabControlItemA.Header + "";
                        break;
                    case 2:
                        bestMedNumber = tabControlItemB.Header + "";
                        break;
                    case 3:
                        bestMedNumber = tabControlItemC.Header + "";
                        break;
                    default:
                        bestMedNumber = "";
                        break;
                }

                labelAnswer.Content = bestMedNumber + " - наиболее эффективный препарат!";

                //TODO: refactor
                info.ShowInfo(series);

                Draw();
                buttonMakeReport.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(comboBoxN.Text);
            int sizeOfSeries = int.Parse(comboBoxNumberOfSeries.Text);

            logic.Save(series, a, size, sizeOfSeries);
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            series = access.Load("Info.xml");

            InitTable(series[0].Size);
            comboBoxN.SelectedValue = series[0].Size;
            comboBoxNumberOfSeries.SelectedValue = series[0].SizeOfSeries;

            logic.Load(series, a);

            Draw();

            //TODO:refactor
            buttonCulc_Click(sender, e);
        }

        private void buttonLoadAs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML documents |*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                series = access.Load(fileName);
                InitTable(series[0].Size);
                comboBoxN.SelectedValue = series[0].Size;
                comboBoxNumberOfSeries.SelectedValue = series[0].SizeOfSeries;

                logic.Load(series, a);

                Draw();
            }
        }

        private void buttonSaveGraph_Click(object sender, RoutedEventArgs e)
        {
            info.Close();
            info = new InfoWindow();
            info.ShowInfo(series);
            info.Show();
        }

        private void buttonRename_Click(object sender, RoutedEventArgs e)
        {
            tabControlItemA.Header = textBoxSeriesName1.Text;
            tabControlItemB.Header = textBoxSeriesName2.Text;
            tabControlItemC.Header = textBoxSeriesName3.Text;

            labelInstr1.Content = "- " + textBoxSeriesName1.Text;
            labelInstr2.Content = "- " + textBoxSeriesName2.Text;
            labelInstr3.Content = "- " + textBoxSeriesName3.Text;
        }

        //TODO: make optimization
        private void buttonMakeReport_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "";

            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.DefaultExt = ".pdf";
            saveFileDialog2.Filter = "PDF files|*.pdf";

            int numberOfSeries = (int)comboBoxNumberOfSeries.SelectedValue;

            if (saveFileDialog2.ShowDialog() == true)
            {
                try
                {
                    fileName = saveFileDialog2.FileName;

                    string nameA = tabControlItemA.Header.ToString();
                    string nameB = tabControlItemB.Header.ToString();
                    string nameC = tabControlItemC.Header.ToString();

                    logic.MakeReport(fileName, nameA, nameB, nameC, series, a, numberOfSeries);
                }
                catch
                {
                    MessageBox.Show("Закройте PDF файл (" + fileName + ") и попробуйте еще раз!", "Сообщение");
                }
            }
        }

        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML documents |*.xml";
            int sizeOfSeries = int.Parse(comboBoxNumberOfSeries.Text);
            int size = int.Parse(comboBoxN.Text);

            logic.Save(series, a, size, sizeOfSeries);

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                //TODO:refactor
                access.Save(fileName, series);
            }
        }

        private void InitTable(int n)
        {
            a[0] = new DataArray(n, n);
            a[1] = new DataArray(n, n);
            a[2] = new DataArray(n, n);

            dataGridA.ItemsSource = a[0].Data.DefaultView;
            dataGridB.ItemsSource = a[1].Data.DefaultView;
            dataGridC.ItemsSource = a[2].Data.DefaultView;

            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < a[k].M; i++)
                    for (int j = 0; j < 12; j++)
                        a[k][i][j] = 0;
            }

            dataGridA.CanUserAddRows = false;
            dataGridB.CanUserAddRows = false;
            dataGridC.CanUserAddRows = false;
        }

        private void comboBoxN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int size = int.Parse(comboBoxN.SelectedItem + "");
            InitTable(size);
        }

        private void canvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        private void comboBoxNumberOfSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int size = int.Parse(comboBoxN.SelectedItem + "");
            InitTable(size);

            if (comboBoxNumberOfSeries.SelectedIndex == 2)
            {
                tabControlItemA.IsEnabled = true;
                tabControlItemB.IsEnabled = true;
                tabControlItemC.IsEnabled = true;
                textBoxSeriesName3.IsEnabled = true;
                textBoxSeriesName2.IsEnabled = true;
                textBoxSeriesName1.IsEnabled = true;
            }

            if (comboBoxNumberOfSeries.SelectedIndex == 1)
            {
                tabControlItemA.IsEnabled = true;
                tabControlItemB.IsEnabled = true;
                tabControlItemC.IsEnabled = false;
                textBoxSeriesName3.IsEnabled = false;
                textBoxSeriesName2.IsEnabled = true;
                textBoxSeriesName1.IsEnabled = true;
            }

            if (comboBoxNumberOfSeries.SelectedIndex == 0)
            {
                tabControlItemA.IsEnabled = true;
                tabControlItemB.IsEnabled = false;
                tabControlItemC.IsEnabled = false;
                textBoxSeriesName3.IsEnabled = false;
                textBoxSeriesName2.IsEnabled = false;
                textBoxSeriesName1.IsEnabled = true;
            }
        }

        private void menuItemFileNew_Click(object sender, RoutedEventArgs e)
        {
            buttonMakeReport.IsEnabled = false;
            int n = int.Parse(comboBoxN.SelectedValue + "");

            for (int i = 0; i < 3; i++)
            {
                series[i] = new Series(n);
                series[i].a = new DataArray(n, n);
                series[i].Size = n;
                series[i].MOValue = int.Parse(textBoxMO.Text);//!!!
                series[i].InitTable();
            }

            InitTable(n);
            info.ShowInfo(series);

            Draw();
        }

        private void menuItemFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void menuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("   Для успешного подсчета биологической\nактивности препарата нужно:\n" +
                "1. Выбрать количество серий препарата\n" +
            "2. Выбрать количество животных в серии\n" +
            "3. Ввести размеры реакций в ячейки таблиц\n" +
            "4. Нажать кнопку \"Считать\"\n" +
            " \n" +
            "   На графике можно увидеть результаты анализа\nданных.\n" +
            "   В случае необходимости можно скоректировать\nкоефициент биологической активности.\n" +
            "   Для удобства, серии в таблице и на графике имеют\nодинаковый цвет.\n", "Помощь");
        }

        private void menuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox a = new AboutBox();
            a.Text = String.Format("О программе");
            a.labelProductName.Text = "Расчет биологической активности препарата";
            a.Show();
        }

        public void Draw()
        {
            int size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");
            List<double> list = new List<double>();

            for (int i = 0; i < size; i++)
            {
                list.Add(series[i].ratio);
            }

            canvasGraph.Children.Clear();
            double xMin = 0;
            double xMax = 6;
            double yMin = 0;

            int border = 30;//отступ

            double yMax = double.Parse(comboBoxN.SelectedValue + "") * 23;
            double width = canvasGraph.ActualWidth;
            double height = canvasGraph.ActualHeight;
            double xScale = width / (xMax - xMin);
            double yScale = height / (yMax - yMin);
            double x0 = (-xMin * xScale) + border;
            double y0 = (yMax * yScale) - border;

            //оси
            AddLine(Brushes.Black, border, height - border, width - border, height - border);
            AddLine(Brushes.Black, border, 20, border, height - border);
            AddText("0", x0 + 2, y0 + 0);
            AddText("Разведение", width - 100, y0 - 20);
            AddText("Размер\nалергической\nреакции (mm)", x0, 0);

            //сетка
            double xStep = 1; //шаг сетки

            while (xStep * xScale < 25)
            {
                xStep *= 10;
            }

            while (xStep * xScale > 250)
            {
                xStep /= 10;
            }

            for (double dx = xStep; dx < xMax; dx += xStep)
            {
                double x = x0 + dx * xScale;
            }

            AddText(string.Format("{0:0.###}", "I"), 1 * xScale + border + 2, y0);
            AddText(string.Format("{0:0.###}", "II"), 4 * xScale + border + 2, y0);

            double yStep = 1;  //шаг сетки

            while (yStep * yScale < 20)
                yStep *= 10;

            while (yStep * yScale > 40)
                yStep /= 3;

            for (double dy = yStep; dy < yMax - (yMax * 0.2); dy += yStep)
            {
                double y = y0 - dy * yScale;
                AddLine(Brushes.LightGray, x0, y, width - border, y);
                AddText(string.Format("{0:0}", dy), x0 - 25, y - 2);
            }

            //графики
            Brush[] br = new Brush[3] { Brushes.Blue, Brushes.Yellow, Brushes.Green };

            for (int i = 0; i < size; i++)
            {
                if (series[i].ratio > 0)//series[i].ratio != -1
                {
                    if (series[i].ratio == list.Max())
                    {
                        canvasGraph.Children.Add(new Line() { X1 = (1 * xScale) + 5 + border, X2 = (4 * xScale) + 5 + border, Y1 = (y0 - series[i].breeding[0] * yScale) + 5, Y2 = (y0 - series[i].breeding[1] * yScale) + 5, Stroke = br[i] });
                    }
                    else
                    {
                        canvasGraph.Children.Add(new Line() { X1 = (1 * xScale) + 5 + border, X2 = (4 * xScale) + 5 + border, Y1 = (y0 - series[i].breeding[0] * yScale) + 5, Y2 = (y0 - series[i].breeding[1] * yScale) + 5, Stroke = Brushes.Gray });
                    }

                    canvasGraph.Children.Add(new Ellipse()
                    {
                        Fill = br[i],
                        Stroke = br[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness((1 * xScale) + border, y0 - series[i].breeding[0] * yScale, 0, 0)
                    });

                    canvasGraph.Children.Add(new Ellipse()
                    {
                        Fill = br[i],
                        Stroke = br[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness((4 * xScale) + border, y0 - series[i].breeding[1] * yScale, 0, 0)
                    });
                }
            }
        }

        private void AddLine(Brush stroke, double x1, double y1, double x2, double y2)
        {
            canvasGraph.Children.Add(new Line() { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, Stroke = stroke });
        }

        private void AddText(string text, double x, double y)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            // Визначення координат блоку. "Приєднані" властивості 
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvasGraph.Children.Add(textBlock);
        }
    }
}

