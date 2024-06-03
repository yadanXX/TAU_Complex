using OxyPlot;
using OxyPlot.Legends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
using System.Xml.Serialization;

namespace TAU_Complex.Pages.Vol1
{
    /// <summary>
    /// Логика взаимодействия для Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public Page6()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", null, "t", "∆Q(t)");
            plotView3.Model = Utils.GetLinearPlotModel("Входной сигнал", null, "t", "Qвх(t)");
            radioButtonStep.IsChecked = true;
        }
        public delegate double Deleg(double value1, double value2);
        private static Random rnd = new Random();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double k, kp, ky, kd, kr, T, T1, T2, Ty, Td, tk;
            double kramp = 0;
            double rand_value = 0;
            double sigma = 0;
            double mu = 0;

            try
            {
                k = Convert.ToDouble(textBoxK.Text.Replace(".", ","));
                kp = Convert.ToDouble(textBoxKp.Text.Replace(".", ","));
                ky = Convert.ToDouble(textBoxKy.Text.Replace(".", ","));
                kd = Convert.ToDouble(textBoxKd.Text.Replace(".", ","));
                kr = Convert.ToDouble(textBoxKr.Text.Replace(".", ","));
                T = Convert.ToDouble(textBoxT.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                Ty = Convert.ToDouble(textBoxTy.Text.Replace(".", ","));
                Td = Convert.ToDouble(textBoxTd.Text.Replace(".", ","));

                if (((bool)(radioButtonRandom.IsChecked) && comboBox1.SelectedIndex == 1))
                {
                    mu = Convert.ToDouble(textBoxG1.Text.Replace(".", ","));
                    sigma = Convert.ToDouble(textBoxG2.Text.Replace(".", ","));
                    if (sigma <= 0) throw new Exception();
                }

                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if ((bool)(radioButtonRamp.IsChecked))
                {
                    kramp = Convert.ToDouble(textBoxRamp.Text.Replace(".", ","));
                }
                if ((bool)(radioButtonRandom.IsChecked))
                {
                    if (comboBox1.SelectedIndex == 0)
                    {
                        rand_value = Convert.ToDouble(textBoxRV1.Text.Replace(".", ","));
                    }
                    else
                    {
                        rand_value = Convert.ToDouble(textBoxRV2.Text.Replace(".", ","));
                    }
                    if (rand_value <= 0) throw new Exception();
                }
                if (tk <= 0 || k <= 0 || kp <= 0 || ky <= 0 || kd <= 0 || kr <= 0 || T <= 0 || T1 <= 0 || T2 <= 0 || Ty <= 0 || Td <= 0) throw new Exception();
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
            List<DataPoint> dataPoints3 = new List<DataPoint>();

            Deleg xv = step;
            if ((bool)radioButtonStep.IsChecked) xv = step;
            else if ((bool)radioButtonRamp.IsChecked) xv = ramp;
            else if ((bool)radioButtonRandom.IsChecked)
            {
                if (comboBox1.SelectedIndex == 0) xv = Random;
                else if (comboBox1.SelectedIndex == 1) xv = NextGaussian;
            }
            double wv1, wv01, wv02, wv03, wv04, wv2, wv3, wv4 = 0, temp01 = 0, temp02 = 0, temp03 = 0, temp04 = 0, temp2 = 0, temp31 = 0, temp32 = 0;
            double enter = 0;
            double randFreq = tk / rand_value;
            double randCurStage = 0;
            for (double i = 0; i < tk; i += Dt)
            {
                if ((bool)radioButtonRandom.IsChecked)
                {
                    if (i >= randCurStage)
                    {
                        enter = xv(mu, sigma);
                        randCurStage += randFreq;
                    }
                }
                else enter = xv(i, kramp);
                (wv01, temp01) = WLink.Dif(enter, k, temp01, Dt);
                (wv02, temp02) = WLink.PropDifDelay(wv01, 1, T, T1, temp02, Dt);
                (wv03, temp03) = WLink.PropDifDelay(wv02, 1, T, T2, temp03, Dt);
                (wv04, temp04) = WLink.Aperiodic(wv03, 1, T, temp04, Dt);
                wv1 = WLink.NonEnertion(enter - wv4, kp);
                (wv2, temp2) = WLink.Aperiodic(wv1 + wv04, ky, Ty, temp2, Dt);
                (wv3, temp31, temp32) = WLink.Integrating(wv2, kd, Td, temp31, temp32, Dt);
                wv4 = WLink.NonEnertion(wv3, kr);
                dataPoints1.Add(new DataPoint(i, wv4));
                dataPoints2.Add(new DataPoint(i, enter - wv4));
                dataPoints3.Add(new DataPoint(i, enter));
            }
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", dataPoints2, "t", "∆Q(t)");
            plotView3.Model = Utils.GetLinearPlotModel("Входной сигнал", dataPoints3, "t", "Qвх(t)");
        }

        private double ramp(double x, double k)
        {
            return x * k;
        }
        private double step(double x, double k)
        {
            return 1;
        }
        double Random(double x, double k)
        {
            return Math.Round(rnd.NextDouble(), 1);
        }
        public static double NextGaussian(double mu, double sigma)
        {
            // рандом по нормальному закону 
            // mu - пик
            // sigma - разброс
            double rand_normal;
            // do
            // {
            //double mu = 0.5;
            // double sigma = mu / 3;
            var u1 = rnd.NextDouble();
            var u2 = rnd.NextDouble();
            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            rand_normal = mu + sigma * rand_std_normal;
            //} while (rand_normal < 0 || rand_normal > 1);
            return Math.Round(rand_normal, 1);
        }


        private void radioButton_Checked(object sender, EventArgs e)
        {
            if ((bool)radioButtonStep.IsChecked)
            {
                HideExept(grid1);
            }
            else if ((bool)radioButtonRamp.IsChecked)
            {
                HideExept(grid2);
            }
            else if ((bool)radioButtonRandom.IsChecked && comboBox1.SelectedIndex == 0)
            {
                HideExept(grid3);
            }
            else if ((bool)radioButtonRandom.IsChecked && comboBox1.SelectedIndex == 1)
            {
                HideExept(grid4);
            }
        }

        private void HideExept(Grid exept)
        {
            grid1.Visibility = Visibility.Collapsed;
            grid2.Visibility = Visibility.Collapsed;
            grid3.Visibility = Visibility.Collapsed;
            grid4.Visibility = Visibility.Collapsed;

            exept.Visibility = Visibility.Visible;
        }
        private void radioButtonStep_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
