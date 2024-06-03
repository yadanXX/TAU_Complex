using OxyPlot.Series;
using OxyPlot;
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
using OxyPlot.Legends;
using System.Diagnostics;

namespace TAU_Complex.Pages.Vol1
{
    /// <summary>
    /// Логика взаимодействия для Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public Page5()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", null, "t", "∆Q(t)");
            radioButtonStep.IsChecked = true;
            radioButtonAmp.IsChecked = true;
        }
        private delegate double Deleg(double value1, double value2);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView2.InvalidatePlot(true);
            double K = 1;
            double Kky = 1;
            double T;
            double tk;
            double Tky = 1;
            double KRamp = 1;
            try
            {
                if ((bool)(radioButtonAmp).IsChecked)
                {
                    K = Convert.ToDouble(textBoxK.Text.Replace(".", ","));
                }
                T = Convert.ToDouble(textBoxT.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                Kky = Convert.ToDouble(textBoxKky.Text.Replace(".", ","));
                if (tk <= 0) throw new Exception();
                if ((bool)(radioButtonDif).IsChecked || (bool)(radioButtonExo).IsChecked)
                {
                    Tky = Convert.ToDouble(textBoxTky.Text.Replace(".", ","));
                }
                if ((bool)(radioButtonRamp).IsChecked)
                {
                    KRamp = Convert.ToDouble(textBoxRamp.Text.Replace(".", ","));
                }
                if (K <= 0 || T <= 0 || Tky <= 0 || Kky <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            double Dt = Properties.Settings.Default.Dt;

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            Deleg xv = NS;
            if ((bool)(radioButtonStep).IsChecked) xv = step;
            else if ((bool)(radioButtonRamp).IsChecked) xv = ramp;


            double wv1 = 0, wv2 = 0, wv3 = 0, temp1 = 0, temp2 = 0, temp31 = 0, temp32 = 0;

            for (double i = 0; i < tk; i += Dt)
            {
                if ((bool)(radioButtonAmp).IsChecked)
                {
                    (wv1) = WLink.NonEnertion(xv(i, KRamp) - wv3, Kky);
                }
                else if ((bool)(radioButtonDif).IsChecked)
                {
                    (wv1, temp1) = WLink.Difdelay(xv(i, KRamp) - wv3, Kky, Tky, 1, temp1, Dt);
                }
                else if ((bool)(radioButtonExo).IsChecked)
                {
                    (wv1, temp1) = WLink.Exodrom(xv(i, KRamp) - wv3, Kky, Tky, 1, temp1, Dt);
                }
                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                (wv3, temp31, temp32) = WLink.Integrating(wv2, 1, 1, temp31, temp32, Dt);
                dataPoints1.Add(new DataPoint(i, wv3));
                dataPoints2.Add(new DataPoint(i, xv(i, KRamp) - wv3));
            }


            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", dataPoints2, "t", "∆Q(t)");
        }
        #region Input Func
        private double ramp(double x, double k)
        {
            return x * k;
        }
        private double step(double x, double k)
        {
            return 1;
        }
        private double NS(double x, double k)
        {
            return 0;
        }
        #endregion

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ((bool)(radioButtonRamp).IsChecked)
            {
                textBoxRamp.IsEnabled = true;
            }
            else
            {
                textBoxRamp.IsEnabled = false;
            }

            if ((bool)(radioButtonAmp).IsChecked)
            {
                textBoxK.IsEnabled = true;
                textBoxTky.IsEnabled = false;
                SchemePic.Source = new BitmapImage(new Uri("pack://application:,,,/TAU_Complex;component/Resources/page5Pics/Схема точности1.jpg"));
            }

            if ((bool)(radioButtonDif).IsChecked)
            {
                textBoxTky.IsEnabled = true;
                SchemePic.Source = new BitmapImage(new Uri("pack://application:,,,/TAU_Complex;component/Resources/page5Pics/Схема точности2.jpg"));
            }            

            if ((bool)(radioButtonExo).IsChecked)
            {
                textBoxTky.IsEnabled = true;
                SchemePic.Source = new BitmapImage(new Uri("pack://application:,,,/TAU_Complex;component/Resources/page5Pics/Схема точности3.jpg"));
            }

        }
    }
}
