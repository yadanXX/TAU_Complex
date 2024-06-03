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
using OxyPlot.Series;

namespace TAU_Complex.Pages.Vol1.Page1
{
    /// <summary>
    /// Логика взаимодействия для Page1_4.xaml
    /// </summary>
    public partial class Page1_4 : Page, SubPageModule1
    {
        public Page1_4(PlotView plot)
        {
            InitializeComponent();
            plotView = plot;
        }
        public PlotModel thisModel { get; set; }
        public PlotView plotView { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView.InvalidatePlot(true);
            double k1, T1, T2, tk;
            try
            {
                k1 = Convert.ToDouble(textBoxk1.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || k1 <= 0 || T1 <= 0 || T2 <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            double Dt = Properties.Settings.Default.Dt;
            
            double xi;
            xi = T1 / (2.0 * T2);
            List<DataPoint> dataPoints = new List<DataPoint>();
            for (double i = 0; i < tk; i += Dt)
            {
                dataPoints.Add(new DataPoint(i, k1 * (1.0 - Math.Exp((-xi * i) / T2) * (Math.Cos(i * Math.Sqrt(1.0 - Math.Pow(xi, 2)) / T2) + xi /
                    (Math.Sqrt(1.0 - Math.Pow(xi, 2))) * Math.Sin(i * Math.Sqrt(1.0 - Math.Pow(xi, 2)) / T2)))));
            }
            plotView.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints, "t", "Qвых(t)");
            thisModel = plotView.Model;
        }
    }
}
