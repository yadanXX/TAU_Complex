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
    public partial class Page7_Vol1 : Page
    {
        public Page7_Vol1()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Q1вых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Q2вых(t)");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double K1, K2, T1, T2, tk, y;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                y = Convert.ToDouble(textBoxY.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || T1 <= 0 || T2 <= 0 || y <= 0 || tk <= 0) throw new Exception();
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

            double xv = 1;
            double sv1 = 0, sv2 = 0, sv3 = 0, sv4 = 0;
            double a = 0, b = 0, c = 0, d = 0;
            double wv1 = 0, wv2 = 0;
            double temp11 = 0, temp12 = 0, temp21 = 0, temp22 = 0;
            for (double i = 0; i < tk; i += Dt)
            {
                sv1 = xv - wv1;
                sv3 = xv - wv2;

                a = Math.Cos(y) * sv1;
                c = -Math.Sin(y) * sv1;
                b = Math.Sin(y) * sv3;
                d = Math.Cos(y) * sv3;

                sv2 = a + b;
                sv4 = c + d;

                (wv1, temp11, temp12) = WLink.Integrating(sv2, K1, T1, temp11, temp12, Dt);
                (wv2, temp21, temp22) = WLink.Integrating(sv4, K2, T2, temp21, temp22, Dt);


                dataPoints1.Add(new DataPoint(i, wv1));
                dataPoints2.Add(new DataPoint(i, wv2));
            }
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Q1вых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints2, "t", "Q2вых(t)");
        }
    }
}
