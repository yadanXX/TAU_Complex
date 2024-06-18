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
    public partial class Page5_Vol2 : Page
    {
        public Page5_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", null, "t", "Q(t)");
            plotView2.Model = Utils.GetLinearPlotModel("Переходная характеристика", null, "t", "Q(t)");
        }

        public delegate double Deleg(double a);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            plotView2.InvalidatePlot(true);
            double k;
            double a1, a2, a3;
            double tk;
            try
            {

                k = Convert.ToDouble(B2.Text.ToString().Replace(".", ","));
                a1 = -1 * Convert.ToDouble(A22.Text.ToString().Replace(".", ","));
                a2 = -1 * Convert.ToDouble(A21.Text.ToString().Replace(".", ","));
                a3 = -1 * Convert.ToDouble(A20.Text.ToString().Replace(".", ","));
                tk = Convert.ToDouble(textBoxtkl.Text.Replace(".", ","));
                if ((bool)FeedBack_RadioButton.IsChecked)
                {

                    a1 += Convert.ToDouble(D2.Text.ToString().Replace(".", ","));
                    a2 += Convert.ToDouble(D1.Text.ToString().Replace(".", ","));
                    a3 += Convert.ToDouble(D0.Text.ToString().Replace(".", ","));
                }
                if (tk <= 0 || a1 <= 0 || a2 <= 0 || a3 <= 0 || k <= 0) throw new Exception();

            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }


            //double Dt = Properties.Settings.Default.Dt;

            double Dt = Data.GetDt(new List<double> { a1,a2,a3 }, tk);

            List<DataPoint> dataPoints1 = new List<DataPoint>();


            double wv1;
            double temp1 = 0, temp2 = 0, temp3 = 0;



            for (double i = 0; i < tk; i += Dt)
            {
                (wv1, temp1, temp2, temp3) = WLink.Mod(1, k, a1, a2, a3, temp1, temp2, temp3, Dt);
                dataPoints1.Add(new DataPoint(i, wv1));
            }

            plotView1.Model = Utils.GetLinearPlotModel("Переходная характеристика", dataPoints1, "t", "Q(t)");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            double K;
            double a1, a2, a3;
            double tk;
            try
            {
                K = Convert.ToDouble(textBoxK.Text.Replace(".", ","));
                a1 = Convert.ToDouble(textBoxa1.Text.Replace(".", ","));
                a2 = Convert.ToDouble(textBoxa2.Text.Replace(".", ","));
                a3 = Convert.ToDouble(textBoxa3.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (tk <= 0 || a1 <= 0 || a2 <= 0 || a3 <= 0 || K <= 0) throw new Exception();

            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }


            //double Dt = Properties.Settings.Default.Dt;

            double Dt = Data.GetDt(new List<double> { a1,a2,a3 }, tk);

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            double wv1;
            double temp1 = 0, temp2 = 0, temp3 = 0;


            for (double i = 0; i < tk; i += Dt)
            {
                (wv1, temp1, temp2, temp3) = WLink.Mod(1, K, a1, a2, a3, temp1, temp2, temp3, Dt);
                dataPoints1.Add(new DataPoint(i, wv1));
            }

            plotView2.Model = Utils.GetLinearPlotModel("Переходная характеристика", dataPoints1, "t", "Q(t)");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (FeedBack_RadioButton == null || NonFeedBack_RadioButton == null)
            {
                return;
            }
            if ((bool)FeedBack_RadioButton.IsChecked)
            {
                LeftModel_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page5_Vol2Pics/В пространстве состояний с обратной связью.png", UriKind.Relative));
                D_StackPanel.Visibility = Visibility.Visible;
            }
            if ((bool)NonFeedBack_RadioButton.IsChecked)
            {
                D_StackPanel.Visibility = Visibility.Collapsed;
                LeftModel_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page5_Vol2Pics/В пространстве состояний без обратной связи.png", UriKind.Relative));
            }
        }
    }
}
