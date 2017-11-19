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
            labelABreed1.Content = series[0].Breeding[0];
            labelABreed2.Content = series[0].Breeding[1];
            labelABreed3.Content = series[0].Breeding[2];
            labelABreed4.Content = series[0].Breeding[3];

            labelBBreed1.Content = series[1].Breeding[0];
            labelBBreed2.Content = series[1].Breeding[1];
            labelBBreed3.Content = series[1].Breeding[2];
            labelBBreed4.Content = series[1].Breeding[3];

            labelCBreed1.Content = series[2].Breeding[0];
            labelCBreed2.Content = series[2].Breeding[1];
            labelCBreed3.Content = series[2].Breeding[2];
            labelCBreed4.Content = series[2].Breeding[3];

            var MOValue = series[0].MOValue;

            var Ad1 = series[0].Breeding[0] + series[0].Breeding[1];
            var Ad2 = series[0].Breeding[2] + series[0].Breeding[3];

            labelAD1.Content = $"d1 = { Ad1 }";
            labelAD2.Content = $"d2 = { Ad2 }";

            var Bd1 = series[1].Breeding[0] + series[1].Breeding[1];
            var Bd2 = series[1].Breeding[2] + series[1].Breeding[3];

            labelBD1.Content = $"d1 = { Bd1 }";
            labelBD2.Content = $"d2 = { Bd2 }";

            var Cd1 = series[2].Breeding[0] + series[2].Breeding[1];
            var Cd2 = series[2].Breeding[2] + series[2].Breeding[3];

            labelCD1.Content = $"d1 = { Cd1 }";
            labelCD2.Content = $"d2 = { Cd2 }";

            labelAA.Content = $"A = { Ad1 } / { Ad2 } * { MOValue } = { Math.Round(series[0].Activity)}";
            labelBA.Content = $"A = { Bd1 } / { Bd2 } * { MOValue } = { Math.Round(series[1].Activity)}";
            labelCA.Content = $"A = { Cd1 } / { Cd2 } * { MOValue } = { Math.Round(series[2].Activity)}";
            
            labelARatio.Content = $"Эффект. = { series[0].Ratio }";
            labelBRatio.Content = $"Эффект. = { series[1].Ratio }";
            labelCRatio.Content = $"Эффект. = { series[2].Ratio }";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}