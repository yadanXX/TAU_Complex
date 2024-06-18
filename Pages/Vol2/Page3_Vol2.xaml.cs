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

namespace TAU_Complex.Pages.Vol2
{
    public partial class Page3_Vol2 : Page
    {
        public Page3_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetRootPlotModel("Карта корней", null, null, "Вещественная ось", "Мнимая ось");
            plotView2.Model = Utils.GetLinearPlotModel("Переходная характеристика/ЛАЧХ", null, "t/Частота (рад/сек)", "Q(t)/Амплитуда (дБ)");
            RadioButton_Checked(null, null);
        }

        List<DataPoint> list_11 = new List<DataPoint>(); // карта корней нули
        List<DataPoint> list_12 = new List<DataPoint>(); // карта корней полюса
        List<DataPoint> list_2 = new List<DataPoint>(); // переходная
        List<DataPoint> list_3 = new List<DataPoint>(); // лачх
        List<DataPoint> list_32 = new List<DataPoint>(); // лачх

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list_11.Clear();
            list_12.Clear();
            list_2.Clear();
            list_3.Clear();
            list_32.Clear();

            (plotView1.Model.Series.FirstOrDefault() as ScatterSeries).Points.Clear();
            (plotView2.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            plotView2.InvalidatePlot(true);
            double tk = 0, K1 = 0, K2 = 0, T1 = 0, T2 = 0, T3 = 0, si = 0;
            try
            {
                K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                T3 = Convert.ToDouble(textBoxT3.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                if (StackPanel_si.Visibility == Visibility.Visible)
                {
                    si = Convert.ToDouble(textBoxsi.Text.Replace(".", ","));
                    if (si <= 0) throw new Exception();
                }
                if (tk <= 0 || K1 <= 0 || K2 <= 0 || T1 <= 0 || T2 <= 0 || T3 <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            //double Dt = Properties.Settings.Default.Dt;
            double Dt = Data.GetDt(new List<double> {T1,T2,T3 }, tk);

            double wv1, wv2 = 0, temp1 = 0, temp2 = 0, temp3 = 0;

            if ((bool)Pic_Radiobuttom_1.IsChecked)
            {
                list_11.Add(new DataPoint(-K1 * K2 / (K1 * K2 * T2), 0));
                list_12.Add(new DataPoint((-1 * (K1 * K2 * T2 + T1 + T3) + Math.Pow(Math.Pow(K1 * K2 * T2 + T1 + T3, 2) - 4 * T1 * T3 * (1 + K1 * K2), 0.5)) / (2 * T1 * T3), 0));
                list_12.Add(new DataPoint((-1 * (K1 * K2 * T2 + T1 + T3) - Math.Pow(Math.Pow(K1 * K2 * T2 + T1 + T3, 2) - 4 * T1 * T3 * (1 + K1 * K2), 0.5)) / (2 * T1 * T3), 0));
                for (double i = 0; i < tk; i += Dt)
                {
                    (wv1, temp1) = WLink.PropDifDelay(1 - wv2, K2, T3, T2, temp1, Dt);
                    (wv2, temp2) = WLink.Aperiodic(wv1, K1, T1, temp2, Dt);

                    list_2.Add(new DataPoint(i, wv2));
                }
                List<double> maxT = new List<double> { 1 / T1, 1 / T2, 1 / T3 };
                for (double i = Math.Pow(10, -3); i < maxT.Max() + 100; i += 0.01)
                {
                    list_3.Add(new DataPoint(i, 20 * Math.Log10(K1 * K2) + 20 * Math.Log10(Math.Pow(1 + T2 * T2 * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(1 + T1 * T1 * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(1 + T3 * T3 * i * i, 0.5))));
                }
                list_32.Add(new DataPoint(1 / T1, InterpolateX(list_3, 1 / T1)));
                list_32.Add(new DataPoint(1 / T2, InterpolateX(list_3, 1 / T2)));
                list_32.Add(new DataPoint(1 / T3, InterpolateX(list_3, 1 / T3)));

            }
            else if ((bool)Pic_Radiobuttom_2.IsChecked)
            {
                List<Complex> polis = GetRootsOfCubicEquations((2 * si * T1 * T3 + Math.Pow(T1, 2)) / (T3 * Math.Pow(T1, 2)), (2 * si * T1 + T3 + K1 * K2 * T2) / (T3 * Math.Pow(T1, 2)), (K1 * K2 + 1) / (T3 * Math.Pow(T1, 2)));
                list_11.Add(new DataPoint(-K1 * K2 / (K1 * K2 * T2), 0));
                foreach (var i in polis) list_12.Add(new DataPoint(i.Real, i.Imaginary));

                for (double i = 0; i < tk; i += Dt)
                {
                    (wv1, temp1) = WLink.PropDifDelay(1 - wv2, K2, T3, T2, temp1, Dt);
                    (wv2, temp2, temp3) = WLink.Oscillatory(wv1, K1, T1, T1 * si * 2, temp2, temp3, Dt);
                    list_2.Add(new DataPoint(i, wv2));
                }

                List<double> maxT = new List<double> { 1 / T1, 1 / T2, 1 / T3, 1 / (T1 * 2 * si) };
                for (double i = Math.Pow(10, -3); i < maxT.Max() + 100; i += 0.01)
                {
                    list_3.Add(new DataPoint(i, 20 * Math.Log10(K1 * K2) + 20 * Math.Log10(Math.Pow(1 + T2 * T2 * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(1 + T1 * T1 * T1 * T1 * i * i * i * i + 4 * si * si * T1 * T1 * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(1 + T3 * T3 * i * i, 0.5))));
                }
                list_32.Add(new DataPoint(1 / T1, InterpolateX(list_3, 1 / T1)));
                list_32.Add(new DataPoint(1 / (T1 * 2 * si), InterpolateX(list_3, 1 / (T1 * 2 * si))));
                list_32.Add(new DataPoint(1 / T2, InterpolateX(list_3, 1 / T2)));
                list_32.Add(new DataPoint(1 / T3, InterpolateX(list_3, 1 / T3)));
            }
            else if ((bool)Pic_Radiobuttom_3.IsChecked)
            {
                List<Complex> polis = GetRootsOfCubicEquations((T1 + T3) / (T1 * T3), (K1 * K2 * T2 + 1) / (T1 * T3), (K1 * K2) / (T1 * T3));
                list_11.Add(new DataPoint(-K1 * K2 / (K1 * K2 * T2), 0));
                foreach (var i in polis) list_12.Add(new DataPoint(i.Real, i.Imaginary));

                for (double i = 0; i < tk; i += Dt)
                {
                    (wv1, temp1) = WLink.PropDifDelay(1 - wv2, K2, T3, T2, temp1, Dt);
                    (wv2, temp2, temp3) = WLink.Integrating(wv1, K1, T1, temp2, temp3, Dt);
                    list_2.Add(new DataPoint(i, wv2));
                }
                List<double> maxT = new List<double> { 1 / T1, 1 / T2, 1 / T3 };
                for (double i = Math.Pow(10, -3); i < maxT.Max() + 100; i += 0.01)
                {
                    list_3.Add(new DataPoint(i, 20 * Math.Log10(K1 * K2) + 20 * Math.Log10(Math.Pow(1 + T2 * T2 * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(i * i + T1 * T1 * i * i * i * i, 0.5)) - 20 * Math.Log10(Math.Pow(1 + T3 * T3 * i * i, 0.5))));
                }
                list_32.Add(new DataPoint(1 / T1, InterpolateX(list_3, 1 / T1)));
                list_32.Add(new DataPoint(1 / T2, InterpolateX(list_3, 1 / T2)));
                list_32.Add(new DataPoint(1 / T3, InterpolateX(list_3, 1 / T3)));
            }

            if ((bool)RadioButton_T.IsChecked)
            {
                plotView2.Model = Utils.GetLinearPlotModel("График переходной характеристики", list_2, "t", "Qвых(t)");
            }
            else if ((bool)Radiobutton_Hz.IsChecked)
            {
                plotView2.Model = Utils.GetLgPlotModel("ЛАЧХ", list_3, list_32, "Частота (рад/сек)", "Амплитуда (дБ)");
            }

            plotView1.Model = Utils.GetRootPlotModel("Карта корней", list_11, list_12, "Вещественная ось", "Мнимая ось");

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Pic_Radiobuttom_1 == null ||
                Pic_Radiobuttom_2 == null ||
                Pic_Radiobuttom_3 == null)
            {
                return;
            }
            if ((bool)Pic_Radiobuttom_1.IsChecked)
            {
                Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page3_Vol2Pics/cхема с апериодическим.png", UriKind.Relative));
            }
            if ((bool)Pic_Radiobuttom_2.IsChecked)
            {
                StackPanel_si.Visibility = Visibility.Visible;
                Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page3_Vol2Pics/cхема с интегрирующим.png", UriKind.Relative));
            }
            else
            {
                StackPanel_si.Visibility = Visibility.Collapsed;
            }
            if ((bool)Pic_Radiobuttom_3.IsChecked)
            {
                Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page3_Vol2Pics/cхема с колебательным.png", UriKind.Relative));
            }
        }
        private double InterpolateX(List<DataPoint> list, double x)
        {
            return list.Select(n => new { n, distance = Math.Abs(n.X - x) }).OrderBy(p => p.distance).FirstOrDefault().n.Y;
        }
        public static List<Complex> GetRootsOfCubicEquations(double a, double b, double c)
        {
            var q = (Math.Pow(a, 2) - 3 * b) / 9;
            var r = (2 * Math.Pow(a, 3) - 9 * a * b + 27 * c) / 54;

            if (Math.Pow(r, 2) < Math.Pow(q, 3))
            {
                var t = Math.Acos(r / Math.Sqrt(Math.Pow(q, 3))) / 3;
                var x1 = -2 * Math.Sqrt(q) * Math.Cos(t) - a / 3;
                var x2 = -2 * Math.Sqrt(q) * Math.Cos(t + (2 * Math.PI / 3)) - a / 3;
                var x3 = -2 * Math.Sqrt(q) * Math.Cos(t - (2 * Math.PI / 3)) - a / 3;
                return new List<Complex> { x1, x2, x3 };
            }
            else
            {
                var A = -Math.Sign(r) * Math.Pow(Math.Abs(r) + Math.Sqrt(Math.Pow(r, 2) - Math.Pow(q, 3)), (1.0 / 3.0));
                var B = (A == 0) ? 0.0 : q / A;

                var x1 = (A + B) - a / 3;
                var x2 = -(A + B) / 2 - (a / 3) + (Complex.ImaginaryOne * Math.Sqrt(3) * (A - B) / 2);
                var x3 = -(A + B) / 2 - (a / 3) - (Complex.ImaginaryOne * Math.Sqrt(3) * (A - B) / 2);

                if (A == B)
                {
                    x2 = -A - a / 3;
                    return new List<Complex> { x1, x2 };
                }
                return new List<Complex> { x1, x2, x3 };
            }
        }

        private void RadioButton_T_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)RadioButton_T.IsChecked)
            {
                plotView2.Model = Utils.GetLinearPlotModel("График переходной характеристики", list_2, "t", "Qвых(t)");
            }
            else if ((bool)Radiobutton_Hz.IsChecked)
            {
                plotView2.Model = Utils.GetLgPlotModel("ЛАЧХ", list_3, list_32, "Частота (рад/сек)", "Амплитуда (дБ)");
            }
        }
    }
}
