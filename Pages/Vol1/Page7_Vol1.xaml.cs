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
            double K1, K2, T1, T2, tk;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || T1 <= 0 || T2 <= 0 || tk <= 0) throw new Exception();
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

            for (double i = 0; i < tk; i += Dt)
            {

                //(wv1, temp11, temp12) = WLink.Integrating(wv2 + wv3, K1, T1, temp11, temp12, Dt);
                //dataPoints1.Add(new DataPoint(i, wv1));
            }
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");
            plotView2.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints2, "t", "Q2вых(t)");
        }
    }
}
