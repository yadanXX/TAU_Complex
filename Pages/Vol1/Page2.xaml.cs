using OxyPlot.Series;
using OxyPlot.Wpf;
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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Годограф Михайлова", null, "u(w)", "jv(w)");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView2.InvalidatePlot(true);
            double K1, K2, K3, T1, T2, tk, Wk;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                K3 = Convert.ToDouble(textBoxK3.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                Wk = Convert.ToDouble(textBoxWk.Text.Replace(".", ","));
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || K3 <= 0 || T1 <= 0 || T2 <= 0 || Wk <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            //double Dt = Properties.Settings.Default.Dt;

            double Dt = Data.GetDt(new List<double> { T1, T2 }, tk);

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            double XNon, XAper, XInte = 0;
            double x1A = 0, x1I = 0, x2I = 0;
            double xv = 1;
            for (double i = 0; i < tk; i += Dt)
            {
                XNon = WLink.NonEnertion(xv - XInte, K1);
                (XAper, x1A) = WLink.Aperiodic(XNon, K2, T1, x1A, Dt);
                (XInte, x1I, x2I) = WLink.Integrating(XAper, K3, T2, x1I, x2I, Dt);
                dataPoints1.Add(new DataPoint(i, XInte));
            }

            //legend1 = $"k1={textBoxK1.Text} k2={textBoxK2.Text} k3={textBoxK3.Text} T1={textBoxT1.Text} T2={textBoxT2.Text} tk={textBoxtk.Text}";
            for (double i = 0; i < Wk; i += Dt)
            {
                dataPoints2.Add(new DataPoint(K1 * K2 * K3 - Math.Pow(i, 2) * (T1 + T2), i - T1 * T2 * Math.Pow(i, 3)));
            }

            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Годограф Михайлова", dataPoints2, "u(w)", "jv(w)");

        }
    }
}
