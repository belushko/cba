using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CBA.BusinessLogic.Logic;
using CBA.BusinessLogic.Models;
using Microsoft.Win32;

namespace CBA.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly DataArray[] a = new DataArray[3];
        private readonly DataManager _dataManager = new DataManager();
        private readonly ReportManager _reportManager = new ReportManager();

        private readonly bool first = true;

        private readonly DataInitialiser logic = new DataInitialiser();
        private readonly Solver solver = new Solver();
        private InfoWindow info = new InfoWindow();
        private Series[] series = new Series[3];

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

            var size = int.Parse(comboBoxN.SelectedValue + string.Empty);
            var numberOfSeries = int.Parse(comboBoxNumberOfSeries.SelectedValue.ToString());

            for (var i = 0; i < 3; i++)
            {
                series[i] = new Series(size)
                {
                    DataArray = new DataArray(size),
                    Size = size,
                    SizeOfSeries = numberOfSeries,
                    //TODO:remove mo valueІ
                    MOValue = int.Parse(textBoxMO.Text)
                };
            }

            series[0].Name = "A";
            series[1].Name = "B";
            series[2].Name = "C";

            InitTable(size);
        }

        private void InitTable(int n)
        {
            for (var i = 0; i < 3; i++)
            {
                series[i] = new Series(n)
                {
                    DataArray = new DataArray(n)
                };

                //TODO: remove
                series[0].Size = n;
                //Series.SizeOfSeries = int.Parse(comboBoxNumberOfSeries.SelectedValue.ToString());
                series[0].MOValue = int.Parse(textBoxMO.Text); //!!!
            }

            series[0].Name = textBoxSeriesName1.Text;
            series[1].Name = textBoxSeriesName2.Text;
            series[2].Name = textBoxSeriesName3.Text;

            dataGridA.ItemsSource = series[0].DataArray.Data.DefaultView;
            dataGridB.ItemsSource = series[1].DataArray.Data.DefaultView;
            dataGridC.ItemsSource = series[2].DataArray.Data.DefaultView;

            for (var k = 0; k < 3; k++)
            {
                for (var i = 0; i < n; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        series[k].DataArray[i][j] = 0;
                    }
                }
            }

            dataGridA.CanUserAddRows = false;
            dataGridB.CanUserAddRows = false;
            dataGridC.CanUserAddRows = false;
        }

        private void buttonFill_Click(object sender, RoutedEventArgs e)
        {
            var size = int.Parse(comboBoxNumberOfSeries.SelectedItem + string.Empty);
            var sizeOfRows = int.Parse(comboBoxN.SelectedItem + string.Empty);

            logic.FillNumbersByDefault(series, size, sizeOfRows);
        }

        private void buttonCulculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var size = int.Parse(comboBoxN.SelectedValue + string.Empty);
                var moValue = int.Parse(textBoxMO.Text);
                var numberOfSeries = 0;

                if (comboBoxNumberOfSeries.SelectedValue != null)
                {
                    numberOfSeries = int.Parse(comboBoxNumberOfSeries.SelectedValue.ToString());
                }
                else
                {
                    numberOfSeries = series[0].SizeOfSeries;
                }

                for (var i = 0; i < numberOfSeries; i++)
                {
                    series[i].Size = size;
                    series[i].SizeOfSeries = numberOfSeries;
                    series[i].MOValue = moValue;
                }

                var best = solver.Culculate(series);
                labelAnswer.Content = $"{ series[best - 1].Name } - наиболее эффективный препарат!";

                //TODO: refactor
                info.ShowInfo(series);

                DrawGraphic();
                buttonMakeReport.IsEnabled = true;
                buttonSaveGraph.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            _dataManager.SaveDataToFile("DefaultSave.xml", series);
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            series = _dataManager.LoadDataFromFile("DefaultSave.xml");
            
            InitData();

            DrawGraphic();
            buttonCulculate_Click(sender, e);
        }

        private void buttonLoadAs_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML documents |*.xml"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var fileName = openFileDialog.FileName;
            series = _dataManager.LoadDataFromFile(fileName);

            InitData();
            DrawGraphic();
        }

        public void InitData()
        {
            comboBoxN.SelectedValue = series[0].Size;
            comboBoxNumberOfSeries.SelectedValue = series[0].SizeOfSeries;

            textBoxSeriesName1.Text = series[0].Name;
            textBoxSeriesName2.Text = series[1].Name;
            textBoxSeriesName3.Text = series[2].Name;

            dataGridA.ItemsSource = series[0].DataArray.Data.DefaultView;
            dataGridB.ItemsSource = series[1].DataArray.Data.DefaultView;
            dataGridC.ItemsSource = series[2].DataArray.Data.DefaultView;
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

            series[0].Name = textBoxSeriesName1.Text;
            series[1].Name = textBoxSeriesName2.Text;
            series[2].Name = textBoxSeriesName3.Text;

            var connectorSign = "- ";

            labelInstr1.Content = connectorSign + textBoxSeriesName1.Text;
            labelInstr2.Content = connectorSign + textBoxSeriesName2.Text;
            labelInstr3.Content = connectorSign + textBoxSeriesName3.Text;
        }

        private void buttonMakeReport_Click(object sender, RoutedEventArgs e)
        {
            var fileName = string.Empty;

            var saveFileDialog2 = new SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "PDF files|*.pdf"
            };

            if (saveFileDialog2.ShowDialog() != true)
                return;

            try
            {
                fileName = saveFileDialog2.FileName;

                //TODO: remove numberOfSeries
                var numberOfSeries = (int)comboBoxNumberOfSeries.SelectedValue;

                _reportManager.MakeReport(fileName, series, numberOfSeries);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Что-то пошло не так. Закройте PDF файл ({ fileName }) и попробуйте еще раз!", "Сообщение");
            }
        }

        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".xml",
                Filter = "XML documents |*.xml"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            var fileName = saveFileDialog.FileName;

            _dataManager.SaveDataToFile(fileName, series);
        }

        private void comboBoxN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var size = int.Parse(comboBoxN.SelectedItem + "");

            InitTable(size);
        }

        private void canvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraphic();
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
            buttonSaveGraph.IsEnabled = false;

            var n = int.Parse(comboBoxN.SelectedValue + "");

            for (var i = 0; i < series[0].SizeOfSeries; i++)
            {
                series[i] = new Series(n);
                series[i].DataArray = new DataArray(n);
                series[i].Size = n;
                series[i].SizeOfSeries = int.Parse(comboBoxNumberOfSeries.SelectedValue.ToString());
                series[i].MOValue = int.Parse(textBoxMO.Text); //!!!
            }

            InitTable(n);
            info.ShowInfo(series);

            DrawGraphic();
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
            var aboutBox = new AboutBox
            {
                Text = "О программе",
                labelProductName = {Text = "Расчет биологической активности препарата"}
            };

            aboutBox.Show();
        }

        public void DrawGraphic()
        {
            var size = 0;

            if (comboBoxNumberOfSeries.SelectedItem != null)
            {
                size = int.Parse(comboBoxNumberOfSeries.SelectedItem + string.Empty);
            }

            if (size == 0)
            {
                size = series[0].SizeOfSeries;
            }

            var list = new List<double>();

            for (var i = 0; i < size; i++)
            {
                list.Add(series[i].Ratio);
            }

            canvasGraph.Children.Clear();

            double xMin = 0;
            double xMax = 6;
            double yMin = 0;
            var yMax = double.Parse(comboBoxN.SelectedValue + string.Empty) * 23;

            var border = 30; //отступ

            var width = canvasGraph.ActualWidth;
            var height = canvasGraph.ActualHeight;
            var xScale = width / (xMax - xMin);
            var yScale = height / (yMax - yMin);
            var x0 = -xMin * xScale + border;
            var y0 = yMax * yScale - border;

            #region DRAW MAIN LINES
            AddLine(Brushes.Black, border, height - border, width - border, height - border);
            AddLine(Brushes.Black, border, 20, border, height - border);
            AddText("0", x0 + 2, y0 + 0);
            AddText("Разведение", width - 100, y0 - 20);
            AddText("Размер\nалергической\nреакции (mm)", x0, 0);
            #endregion

            #region DRAW GRID
            double xStep = 1; //шаг сетки

            while (xStep * xScale < 25)
            {
                xStep *= 10;
            }

            while (xStep * xScale > 250)
            {
                xStep /= 10;
            }

            for (var dx = xStep; dx < xMax; dx += xStep)
            {
                var x = x0 + dx * xScale;
            }

            AddText("I", 1 * xScale + border + 2, y0);
            AddText("II", 4 * xScale + border + 2, y0);

            double yStep = 1; //шаг сетки

            while (yStep * yScale < 20)
                yStep *= 10;

            while (yStep * yScale > 40)
                yStep /= 3;

            for (var dy = yStep; dy < yMax - yMax * 0.2; dy += yStep)
            {
                var y = y0 - dy * yScale;
                AddLine(Brushes.LightGray, x0, y, width - border, y);
                AddText(string.Format("{0:0}", dy), x0 - 20, y - 2);
            }
            #endregion

            #region DRAW GRAPH
            var brush = new Brush[3] { Brushes.Blue, Brushes.Yellow, Brushes.Green };

            for (var i = 0; i < size; i++)
            {
                if (series[i].Ratio > 0) //series[i].Ratio != -1
                {
                    if (series[i].Ratio == list.Max())
                    {
                        canvasGraph.Children.Add(new Line
                        {
                            X1 = 1 * xScale + 5 + border,
                            X2 = 4 * xScale + 5 + border,
                            Y1 = y0 - series[i].Breeding[0] * yScale + 5,
                            Y2 = y0 - series[i].Breeding[1] * yScale + 5,
                            Stroke = brush[i]
                        });
                    }
                    else
                    {
                        canvasGraph.Children.Add(new Line
                        {
                            X1 = 1 * xScale + 5 + border,
                            X2 = 4 * xScale + 5 + border,
                            Y1 = y0 - series[i].Breeding[0] * yScale + 5,
                            Y2 = y0 - series[i].Breeding[1] * yScale + 5,
                            Stroke = Brushes.Gray
                        });
                    }

                    canvasGraph.Children.Add(new Ellipse
                    {
                        Fill = brush[i],
                        Stroke = brush[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness(1 * xScale + border, y0 - series[i].Breeding[0] * yScale, 0, 0)
                    });

                    canvasGraph.Children.Add(new Ellipse
                    {
                        Fill = brush[i],
                        Stroke = brush[i],
                        Width = 10,
                        Height = 10,
                        Margin = new Thickness(4 * xScale + border, y0 - series[i].Breeding[1] * yScale, 0, 0)
                    });
                }
            }
            #endregion
        }

        private void AddLine(Brush stroke, double x1, double y1, double x2, double y2)
        {
            canvasGraph.Children.Add(new Line
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                Stroke = stroke
            });
        }

        private void AddText(string text, double x, double y)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Foreground = Brushes.Black
            };

            //Определение координат блока
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvasGraph.Children.Add(textBlock);
        }
    }
}