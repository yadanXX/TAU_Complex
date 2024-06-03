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
using System.Collections;

namespace TAU_Complex.Pages.Vol1
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Годограф Найквиста", null, "u(w)", "jv(w)");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView2.InvalidatePlot(true);
            double K1, K2, T1, T2, tk, Wk;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                Wk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || T1 <= 0 || T2 <= 0) throw new Exception();
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

            double XAper = 0, XInt = 0;
            double x1A = 0, x2A = 0, x1I = 0, x2I = 0;
            double xv = 1;
            if (comboBox.Text == "0")
            {
                for (double i = 0; i < tk; i += Dt)
                {
                    (XAper, x1A) = WLink.Aperiodic(xv - XAper, K1, T1, x1A, Dt);
                    (XAper, x2A) = WLink.Aperiodic(XAper, K2, T2, x2A, Dt);
                    dataPoints1.Add(new DataPoint(i, XAper));
                }
                double u, v, deter; // u - действительная часть, v - мнимая, deter - общий знаменатель

                for (double i = 0; i < Wk; i += Dt)
                {
                    u = K1 * K2 * (1d - Math.Pow(i, 2) * T1 * T2);
                    v = -i * K1 * K2 * (T1 + T2);
                    deter = (Math.Pow(i, 2) * Math.Pow(T1, 2) + 1d) * (Math.Pow(i, 2) * Math.Pow(T2, 2) + 1d);
                    dataPoints2.Add(new DataPoint(u / deter, v / deter));
                }

            }
            else
            {
                for (double i = 0; i < tk; i += Dt)
                {
                    (XAper, x1A) = WLink.Aperiodic(xv - XInt, K1, T1, x1A, Dt);
                    (XInt, x1I, x2I) = WLink.Integrating(XAper, K2, T2, x1I, x2I, Dt);
                    dataPoints1.Add(new DataPoint(i, XInt));
                }
                double u, v, deter; // u - действительная часть, v - мнимая, deter - общий знаменатель
                for (double i = 0; i < Wk; i += 0.01)
                {
                    u = -K1 * K2 * (T1 + T2);
                    v = -K1 * K2 * (1d - Math.Pow(i, 2) * T1 * T2);
                    deter = (Math.Pow(i, 2) * Math.Pow(T1, 2) + 1d) * (Math.Pow(i, 2) * Math.Pow(T2, 2) + 1d);
                    dataPoints2.Add(new DataPoint(u / deter, v / (deter * i)));
                }
            }
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Годограф Найквиста", dataPoints2, "u(w)", "jv(w)");
        }
    }
}
