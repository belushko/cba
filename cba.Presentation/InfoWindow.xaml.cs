using System;
using System.Windows;
using CBA.BusinessLogic.Logic;
using CBA.BusinessLogic.Models;

namespace CBA.Presentation
{
    /// <summary>
    ///     Interaction logic for Info.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        public void ShowInfo(Series[] series)
        {
            labelABreed1.Content = series[0].breeding[0] + "";
            labelABreed2.Content = series[0].breeding[1] + "";
            labelABreed3.Content = series[0].breeding[2] + "";
            labelABreed4.Content = series[0].breeding[3] + "";

            labelBBreed1.Content = series[1].breeding[0] + "";
            labelBBreed2.Content = series[1].breeding[1] + "";
            labelBBreed3.Content = series[1].breeding[2] + "";
            labelBBreed4.Content = series[1].breeding[3] + "";

            labelCBreed1.Content = series[2].breeding[0] + "";
            labelCBreed2.Content = series[2].breeding[1] + "";
            labelCBreed3.Content = series[2].breeding[2] + "";
            labelCBreed4.Content = series[2].breeding[3] + "";

            var Ad1 = series[0].breeding[0] + series[0].breeding[1];
            var Ad2 = series[0].breeding[2] + series[0].breeding[3];
            labelAD1.Content = "d1 = " + Ad1 + "";
            labelAD2.Content = "d2 = " + Ad2 + "";

            var Bd1 = series[1].breeding[0] + series[1].breeding[1];
            var Bd2 = series[1].breeding[2] + series[1].breeding[3];
            labelBD1.Content = "d1 = " + Bd1 + "";
            labelBD2.Content = "d2 = " + Bd2 + "";

            var Cd1 = series[2].breeding[0] + series[2].breeding[1];
            var Cd2 = series[2].breeding[2] + series[2].breeding[3];
            labelCD1.Content = "d1 = " + Cd1 + "";
            labelCD2.Content = "d2 = " + Cd2 + "";

            labelAA.Content = "A = " + Ad1 + " / " + Ad2 + " * " + series[0].MOValue + " = " +
                              Math.Round(series[0].activity);
            labelBA.Content = "A = " + Bd1 + " / " + Bd2 + " * " + series[0].MOValue + " = " +
                              Math.Round(series[1].activity);
            labelCA.Content = "A = " + Cd1 + " / " + Cd2 + " * " + series[0].MOValue + " = " +
                              Math.Round(series[2].activity);

            labelARatio.Content = "Эффект. = " + series[0].ratio;
            labelBRatio.Content = "Эффект. = " + series[1].ratio;
            labelCRatio.Content = "Эффект. = " + series[2].ratio;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}