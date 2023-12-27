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

namespace TAU_Complex.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public Page4()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("АФЧХ", null, "u(w)", "jv(w)");
            plotView3.Model = Utils.GetLinearPlotModel("Область устойчивости", null, "K", "T");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView2.InvalidatePlot(true);
            (plotView3.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView3.InvalidatePlot(true);
            double K, T, tau, tk, Wk;
            try
            {
                K = Convert.ToDouble(textBoxK.Text.Replace(".", ","));
                T = Convert.ToDouble(textBoxT.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                tau = Convert.ToDouble(textBoxtau.Text.Replace(".", ","));
                Wk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || T <= 0 || K <= 0 || tau <= 0 || Wk <= 0) throw new Exception();
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

            double XInt = 0;
            double x1I = 0, x2I = 0;
            double xv = 1;
            double err = 0;
            List<double> delay = new List<double>();
            for (double i = 0; i < tk; i += Dt)
            {
                (XInt, x1I, x2I) = WLink.Integrating(xv - err, K, T, x1I, x2I, Dt);
                delay.Add(XInt);
                if (delay.Count >= tau / Dt)
                {
                    err = delay[0];
                    dataPoints1.Add(new DataPoint( i, delay[0]));
                    delay.RemoveAt(0);
                }
            }
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");

            double u = 0, v = 0, deter = 0;

            for (double i = 0; i < Wk; i += Dt)
            {
                u = -K * (Math.Sin(tau * i) + T * i * Math.Cos(tau * i));
                v = -K * (Math.Cos(tau * i) - T * i * Math.Sin(tau * i));
                deter = Math.Pow(T, 2) * Math.Pow(i, 3) + i;
                dataPoints2.Add(new DataPoint( u / deter, v / deter));
            }
            plotView2.Model = Utils.GetLinearPlotModel("АФЧХ", dataPoints2, "u(w)", "jv(w)");

            double sus_k, sus_t;
            for (double i = 0; i < Math.PI / (2 * tau); i += Dt)
            {
                sus_k = i / (Math.Sin(tau * i));
                sus_t = 1d / (i * Math.Tan(tau * i));
                //if (sus_k < 0 || sus_t < 0)
                //{
                //    break;
                //}
                dataPoints2.Add(new DataPoint(sus_k, sus_t));
            }
            plotView3.Model = Utils.GetLinearPlotModel("Область устойчивости", dataPoints3, "K", "T");
        }
    }
}
