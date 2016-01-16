using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using cba.Logic;
using Microsoft.Win32;

namespace cba
{
    public partial class MainWindow : Window
    {
        private readonly DataArray[] a = new DataArray[3];
        private readonly bool first = true;
        private InfoWindow info = new InfoWindow();

        private readonly MainLogic logic = new MainLogic();
        private Series[] series = new Series[3];
        private Solver solver = new Solver();
        private readonly Access access = new Access();

        public MainWindow()
        {
            info.Close();
            InitializeComponent();

            for (var i = 2; i <= 15; i++)
                comboBoxN.Items.Add(i);
            comboBoxN.SelectedItem = 5;

            for (var i = 1; i <= 3; i++)
                comboBoxNumberOfSeries.Items.Add(i);
            comboBoxNumberOfSeries.SelectedItem = 3;

            var size = int.Parse(comboBoxN.SelectedValue + "");

            for (var i = 0; i < 3; i++)
            {
                series[i] = new Series(size);
                series[i].a = new DataArray(size);
                Series.Size = size;
                series[i].MOValue = int.Parse(textBoxMO.Text); //!!!
                series[i].InitTable();
            }

            series[0].Name = "A";
            series[1].Name = "B";
            series[2].Name = "C";

            InitTable(size);
        }

        private void InitTable(int n)
        {
            //a[0] = new DataArray(n);
            //a[1] = new DataArray(n);
            //a[2] = new DataArray(n);

            //dataGridA.ItemsSource = a[0].Data.DefaultView;
            //dataGridB.ItemsSource = a[1].Data.DefaultView;
            //dataGridC.ItemsSource = a[2].Data.DefaultView;

            //for (var k = 0; k < 3; k++)
            //{
            //    for (var i = 0; i < n; i++)
            //        for (var j = 0; j < 12; j++)
            //            a[k][i][j] = 0;
            //}

            //dataGridA.CanUserAddRows = false;
            //dataGridB.CanUserAddRows = false;
            //dataGridC.CanUserAddRows = false;

            for (var i = 0; i < 3; i++)
            {
                series[i] = new Series(n);
                series[i].a = new DataArray(n);
                Series.Size = n;
                series[i].MOValue = int.Parse(textBoxMO.Text); //!!!
                series[i].InitTable();
            }

            series[0].a = new DataArray(n);
            series[1].a = new DataArray(n);
            series[2].a = new DataArray(n);

            dataGridA.ItemsSource = series[0].a.Data.DefaultView;
            dataGridB.ItemsSource = series[0].a.Data.DefaultView;
            dataGridC.ItemsSource = series[0].a.Data.DefaultView;
            
            for (var k = 0; k < 3; k++)
            {
                for (var i = 0; i < n; i++)
                    for (var j = 0; j < 12; j++)
                    {
                        series[0].a[i][j] = 0;
                        //a[k][i][j] = 0;
                    }
            }

            dataGridA.CanUserAddRows = false;
            dataGridB.CanUserAddRows = false;
            dataGridC.CanUserAddRows = false;
        }

        private void buttonFill_Click(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");
            var sizeOfRows = int.Parse(comboBoxN.SelectedItem + "");

            logic.FillNumbers(series, size, sizeOfRows);
        }

        private void buttonCulc_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                var size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");
                var moValue = int.Parse(textBoxMO.Text);

                var best = logic.Culculate(series, a, moValue, first, size);

                var bestMedNumber = "";

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
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(comboBoxN.Text);
            var sizeOfSeries = int.Parse(comboBoxNumberOfSeries.Text);

            logic.Save(series, a, size, sizeOfSeries);
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            series = access.Load("Info.xml");

            InitTable(Series.Size);
            comboBoxN.SelectedValue = Series.Size;
            comboBoxNumberOfSeries.SelectedValue = Series.SizeOfSeries;

            logic.Load(series, a);

            Draw();

            //TODO:refactor
            buttonCulc_Click(sender, e);
        }

        private void buttonLoadAs_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML documents |*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                series = access.Load(fileName);
                InitTable(Series.Size);
                comboBoxN.SelectedValue = Series.Size;
                comboBoxNumberOfSeries.SelectedValue = Series.SizeOfSeries;

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
            var fileName = "";

            var saveFileDialog2 = new SaveFileDialog();
            saveFileDialog2.DefaultExt = ".pdf";
            saveFileDialog2.Filter = "PDF files|*.pdf";

            var numberOfSeries = (int) comboBoxNumberOfSeries.SelectedValue;

            if (saveFileDialog2.ShowDialog() == true)
            {
                try
                {
                    fileName = saveFileDialog2.FileName;

                    var nameA = tabControlItemA.Header.ToString();
                    var nameB = tabControlItemB.Header.ToString();
                    var nameC = tabControlItemC.Header.ToString();

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
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML documents |*.xml";
            var sizeOfSeries = int.Parse(comboBoxNumberOfSeries.Text);
            var size = int.Parse(comboBoxN.Text);

            logic.Save(series, a, size, sizeOfSeries);

            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;

                //TODO:refactor
                access.Save(fileName, series);
            }
        }

        private void comboBoxN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var size = int.Parse(comboBoxN.SelectedItem + "");
            InitTable(size);
        }

        private void canvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        private void comboBoxNumberOfSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var size = int.Parse(comboBoxN.SelectedItem + "");
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
            var n = int.Parse(comboBoxN.SelectedValue + "");

            for (var i = 0; i < 3; i++)
            {
                series[i] = new Series(n);
                series[i].a = new DataArray(n);
                Series.Size = n;
                series[i].MOValue = int.Parse(textBoxMO.Text); //!!!
                series[i].InitTable();
            }

            InitTable(n);
            info.ShowInfo(series);

            Draw();
        }

        private void menuItemFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            var a = new AboutBox();
            a.Text = "О программе";
            a.labelProductName.Text = "Расчет биологической активности препарата";
            a.Show();
        }

        public void Draw()
        {
            var size = int.Parse(comboBoxNumberOfSeries.SelectedItem + "");
            var list = new List<double>();

            for (var i = 0; i < size; i++)
            {
                list.Add(series[i].ratio);
            }

            canvasGraph.Children.Clear();
            double xMin = 0;
            double xMax = 6;
            double yMin = 0;

            var border = 30; //отступ

            var yMax = double.Parse(comboBoxN.SelectedValue + "")*23;
            var width = canvasGraph.ActualWidth;
            var height = canvasGraph.ActualHeight;
            var xScale = width/(xMax - xMin);
            var yScale = height/(yMax - yMin);
            var x0 = -xMin*xScale + border;
            var y0 = yMax*yScale - border;

            //оси
            AddLine(Brushes.Black, border, height - border, width - border, height - border);
            AddLine(Brushes.Black, border, 20, border, height - border);
            AddText("0", x0 + 2, y0 + 0);
            AddText("Разведение", width - 100, y0 - 20);
            AddText("Размер\nалергической\nреакции (mm)", x0, 0);

            //сетка
            double xStep = 1; //шаг сетки

            while (xStep*xScale < 25)
            {
                xStep *= 10;
            }

            while (xStep*xScale > 250)
            {
                xStep /= 10;
            }

            for (var dx = xStep; dx < xMax; dx += xStep)
            {
                var x = x0 + dx*xScale;
            }

            AddText(string.Format("{0:0.###}", "I"), 1*xScale + border + 2, y0);
            AddText(string.Format("{0:0.###}", "II"), 4*xScale + border + 2, y0);

            double yStep = 1; //шаг сетки

            while (yStep*yScale < 20)
                yStep *= 10;

            while (yStep*yScale > 40)
                yStep /= 3;

            for (var dy = yStep; dy < yMax - yMax*0.2; dy += yStep)
            {
                var y = y0 - dy*yScale;
                AddLine(Brushes.LightGray, x0, y, width - border, y);
                AddText(string.Format("{0:0}", dy), x0 - 20, y - 2);
            }

            //графики
            var br = new Brush[3] {Brushes.Blue, Brushes.Yellow, Brushes.Green};

            for (var i = 0; i < size; i++)
            {
                if (series[i].ratio > 0) //series[i].ratio != -1
                {
                    if (series[i].ratio == list.Max())
                    {
                        canvasGraph.Children.Add(new Line
                        {
                            X1 = 1*xScale + 5 + border,
                            X2 = 4*xScale + 5 + border,
                            Y1 = y0 - series[i].breeding[0]*yScale + 5,
                            Y2 = y0 - series[i].breeding[1]*yScale + 5,
                            Stroke = br[i]
                        });
                    }
                    else
                    {
                        canvasGraph.Children.Add(new Line
                        {
                            X1 = 1*xScale + 5 + border,
                            X2 = 4*xScale + 5 + border,
                            Y1 = y0 - series[i].breeding[0]*yScale + 5,
                            Y2 = y0 - series[i].breeding[1]*yScale + 5,
                            Stroke = Brushes.Gray
                        });
                    }

                    canvasGraph.Children.Add(new Ellipse
                    {
                        Fill = br[i],
                        Stroke = br[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness(1*xScale + border, y0 - series[i].breeding[0]*yScale, 0, 0)
                    });

                    canvasGraph.Children.Add(new Ellipse
                    {
                        Fill = br[i],
                        Stroke = br[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness(4*xScale + border, y0 - series[i].breeding[1]*yScale, 0, 0)
                    });
                }
            }
        }

        private void AddLine(Brush stroke, double x1, double y1, double x2, double y2)
        {
            canvasGraph.Children.Add(new Line {X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, Stroke = stroke});
        }

        private void AddText(string text, double x, double y)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = Brushes.Black;
            // Визначення координат блоку. "Приєднані" властивості 
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvasGraph.Children.Add(textBlock);
        }
    }
}