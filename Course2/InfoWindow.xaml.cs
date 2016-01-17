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
using System.Windows.Shapes;
using cba;
using cba.Logic;

namespace cba
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
            
        }
        public void ShowInfo(Series [] series)
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

            double Ad1=series[0].breeding[0]+series[0].breeding[1];
            double Ad2=series[0].breeding[2]+series[0].breeding[3];
            labelAD1.Content = "d1 = "+Ad1 + "";
            labelAD2.Content = "d2 = " + Ad2 + "";

            double Bd1 = series[1].breeding[0] + series[1].breeding[1];
            double Bd2 = series[1].breeding[2] + series[1].breeding[3];
            labelBD1.Content = "d1 = " + Bd1 + "";
            labelBD2.Content = "d2 = " + Bd2 + "";

            double Cd1 = series[2].breeding[0] + series[2].breeding[1];
            double Cd2 = series[2].breeding[2] + series[2].breeding[3];
            labelCD1.Content = "d1 = " + Cd1 + "";
            labelCD2.Content = "d2 = " + Cd2 + "";

            labelAA.Content = "A = " + Ad1 + " / " + Ad2 + " * " + Series.MOValue + " = " + Math.Round(series[0].activity);
            labelBA.Content = "A = " + Bd1 + " / " + Bd2 + " * " + Series.MOValue + " = " + Math.Round(series[1].activity);
            labelCA.Content = "A = " + Cd1 + " / " + Cd2 + " * " + Series.MOValue + " = " + Math.Round(series[2].activity);

            labelARatio.Content = "Эффект. = " + series[0].ratio;
            labelBRatio.Content = "Эффект. = " + series[1].ratio;
            labelCRatio.Content = "Эффект. = " + series[2].ratio;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
