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
using System.Security.Policy;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
using System.Numerics;
using static TAU_Complex.Pages.Vol1.Page6;

namespace TAU_Complex.Pages.Vol2
{
    public partial class Page4_Vol2 : Page
    {
        public Page4_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", null, "t", "Q(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", null, "t", "∆Q(t)");
        }

        public delegate double Deleg(double a);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            plotView2.InvalidatePlot(true);
            double tk = 0, K1 = 0, K2 = 0, K3 = 0, K4 = 0, T3 = 0, T4 = 0;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                K3 = Convert.ToDouble(textBoxK3.Text.Replace(".", ","));
                K4 = Convert.ToDouble(textBoxK4.Text.Replace(".", ","));
                T3 = Convert.ToDouble(textBoxT3.Text.Replace(".", ","));
                T4 = Convert.ToDouble(textBoxT4.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || K3 <= 0 || K4 <= 0 || T3 <= 0 || T4 <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            //double Dt = Properties.Settings.Default.Dt;

            double Dt = Data.GetDt(new List<double> {T3, T4 }, tk);

            List<DataPoint> dataPoints1 = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            Deleg xv = NS;
            if ((bool)Signal_Radiobuttom_1.IsChecked) xv = step;
            else if ((bool)Signal_Radiobuttom_2.IsChecked) xv = sinus;

            double wv1, wv2, wv3, wv4 = 0, wv5 = 0;
            double temp3 = 0, temp41 = 0, temp42 = 0;

            for (double i = 0; i < tk; i += Dt)
            {
                dataPoints2.Add(new DataPoint(i, xv(i) - wv5));
                wv1 = WLink.NonEnertion(xv(i) - wv5, K1);
                wv2 = wv1 - WLink.NonEnertion(wv4, K2);
                if (wv2 <= -1 || wv2 >= 1)
                {
                    wv2 = wv2 / Math.Abs(wv2);
                }

                (wv3, temp3) = WLink.Aperiodic(wv2, K3, T3, temp3, Dt);
                (wv4, temp41, temp42) = WLink.Oscillatory(wv3, K4, T4, T4, temp41, temp42, Dt);
                wv5 = WLink.IdealInter(wv4, 1, wv5, Dt);
                dataPoints1.Add(new DataPoint(i, wv5));
            }

            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", dataPoints1, "t", "Q(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Ошибка", dataPoints2, "t", "∆Q(t)");
        }

        double sinus(double time)
        {
            return Math.Sin(0.5 * time);
        }
        double step(double a)
        {
            return 1;
        }
        double NS(double a)
        {
            return 0;
        }
    }
}
