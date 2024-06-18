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
    public partial class Page6_Vol2 : Page
    {
        public Page6_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", null, "t", "Q(t)");
        }

        public delegate double Deleg(double a);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            double K;
            double T1, T2;
            double tk, D;
            try
            {
                K = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                D = Convert.ToDouble(textBoxDt.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || K <= 0 || T1 <= 0 || T2 <= 0 || D <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            //double Dt = Properties.Settings.Default.Dt;
            //double Dt = Data.GetDt(new List<double> { T1, T2 }, tk);

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            dataPoints1.Add(new DataPoint(0, 0));
            dataPoints1.Add(new DataPoint(D, 0));


            for (double i = D * 2; i < tk; i += D)
            {
                dataPoints1.Add(new DataPoint(i, dataPoints1.Last().Y));
                dataPoints1.Add(new DataPoint(i, -T1 * dataPoints1[dataPoints1.Count - 2].Y - (T2 + K) * dataPoints1[dataPoints1.Count - 3].Y + K));
            }

            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", dataPoints1, "t", "Q(t)");
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
