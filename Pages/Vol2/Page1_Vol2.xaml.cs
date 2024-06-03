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

namespace TAU_Complex.Pages.Vol2
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page1_Vol2 : Page
    {
        private int modelNumber = 1;
        public Page1_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            RadioButton_Checked(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            double K1=0, K2 = 0, K3=0, T1 = 0, T2=0, tk = 0, Tnu=0, si=0;
            try
            {
                if (StackPanel_K1.Visibility == Visibility.Visible)
                {
                    K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                    if (K1 <= 0) throw new Exception();
                }
                if (StackPanel_K2.Visibility == Visibility.Visible)
                {
                    K2 = Convert.ToDouble(textBoxK2.Text.Replace(".", ","));
                    if (K2 <= 0) throw new Exception();
                }
                if (StackPanel_K3.Visibility == Visibility.Visible)
                {
                    K3 = Convert.ToDouble(textBoxK3.Text.Replace(".", ","));
                    if (K3 <= 0) throw new Exception();
                }
                if (StackPanel_T1.Visibility == Visibility.Visible)
                {
                    T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                    if (T1 <= 0) throw new Exception();
                }
                if (StackPanel_T2.Visibility == Visibility.Visible)
                {
                    T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                    if (T2 <= 0) throw new Exception();
                }
                if (StackPanel_Tnu.Visibility == Visibility.Visible)
                {
                    Tnu = Convert.ToDouble(textBoxTnu.Text.Replace(".", ","));
                    if (Tnu <= 0) throw new Exception();
                }
                if (StackPanel_si.Visibility == Visibility.Visible)
                {
                    si = Convert.ToDouble(textBoxsi.Text.Replace(".", ","));
                    if (si <= 0) throw new Exception();
                }
                if (StackPanel_tk.Visibility == Visibility.Visible)
                {
                    tk = Convert.ToDouble(textBoxtk.Text.Replace(".", ","));
                    if (tk <= 0) throw new Exception();
                }
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            double Dt = Properties.Settings.Default.Dt;

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            double xv = 1;
            double wv1 = 0, wv2 = 0, wv3 = 0, temp1 = 0, temp2 = 0, temp11 = 0, temp12 = 0;

            switch (modelNumber)
            {
                case 0:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        (wv1, temp1) = WLink.Aperiodic(xv - wv1, K1, T1, temp1, Dt);
                        dataPoints1.Add(new DataPoint(i, wv1));
                        wv2 = wv1 * K2;
                        wv1 = wv1 - wv2;

                    }
                    break;
                case 1:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        wv1 = wv1 - wv2;
                        (wv1, temp1) = WLink.Aperiodic(xv - wv1, K1, T1, temp1, Dt);
                        dataPoints1.Add(new DataPoint(i, wv1));
                        (wv2, temp2) = WLink.PropDifDelay(wv1, 1 / K2, Tnu, T2, temp2, Dt);

                    }
                    break;
                case 2:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        wv2 = WLink.IdealInter(xv - wv1, K2, wv2, Dt);
                        wv3 = WLink.NonEnertion(xv - wv1, K3);
                        (wv1, temp1) = WLink.Aperiodic(wv2 + wv3, K1, T1, temp1, Dt);
                        dataPoints1.Add(new DataPoint(i, wv1));
                    }
                    break;
                case 3:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        wv2 = WLink.IdealInter(xv - wv1, K2, wv2, Dt);
                        wv3 = WLink.NonEnertion(xv - wv1, K3);
                        (wv1, temp11, temp12) = WLink.Oscillatory(wv2 + wv3, K1, T1, 2 * si * T1, temp11, temp12, Dt);
                        dataPoints1.Add(new DataPoint(i, wv1));
                    }
                    break;
                case 4:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        wv2 = WLink.IdealInter(xv - wv1, K2, wv2, Dt);
                        wv3 = WLink.NonEnertion(xv - wv1, K3);
                        (wv1, temp11, temp12) = WLink.Integrating(wv2 + wv3, K1, T1, temp11, temp12, Dt);
                        dataPoints1.Add(new DataPoint(i, wv1));
                    }
                    break;
            }
            //legend1 = $"K1={textBoxK1.Text} K2={textBoxK2.Text} K3={textBoxK3.Text} T1={textBoxT1.Text} T2={textBoxT2.Text} tk={textBoxtk.Text}";

            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (NSFB_RadioButton == null ||
                SOL_RadioButton == null ||
                HFB_RadioButton == null ||
                FB_RadioButton == null ||
                Pic_Radiobuttom_1 == null ||
                Pic_Radiobuttom_2 == null ||
                Pic_Radiobuttom_3 == null)
            {
                return;
            }
            if ((bool)NSFB_RadioButton.IsChecked)
            {
                TypeLink_GroupBox.Visibility = Visibility.Visible;
                Object_GroupBox.Visibility = Visibility.Collapsed;
                if ((bool)HFB_RadioButton.IsChecked)
                {
                    SetModel(0);
                }
                else if ((bool)FB_RadioButton.IsChecked)
                {
                    SetModel(1);
                }
            }
            else if ((bool)SOL_RadioButton.IsChecked)
            {
                Object_GroupBox.Visibility = Visibility.Visible;
                TypeLink_GroupBox.Visibility = Visibility.Collapsed;
                if ((bool)Pic_Radiobuttom_1.IsChecked)
                {
                    SetModel(2);
                }
                else if ((bool)Pic_Radiobuttom_2.IsChecked)
                {
                    SetModel(3);
                }
                else if ((bool)Pic_Radiobuttom_3.IsChecked)
                {
                    SetModel(4);
                }
            }
        }
        private void SetModel(int modelNumber)
        {
            switch (modelNumber)
            {
                case 0:
                    Model_Image.Source = new BitmapImage(new Uri("/Resources/Page1_Vol2Pics/Схема САР-1.jpg", UriKind.Relative));
                    CollapseAllVariables();
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_K2.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    break;
                case 1:
                    Model_Image.Source = new BitmapImage(new Uri("/Resources/Page1_Vol2Pics/Схема САР-2.jpg", UriKind.Relative));
                    CollapseAllVariables();
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_K2.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_T2.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    break;
                case 2:
                    Model_Image.Source = new BitmapImage(new Uri("/Resources/Page1_Vol2Pics/Схема САР-3.jpg", UriKind.Relative));
                    CollapseAllVariables();
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_K2.Visibility = Visibility.Visible;
                    StackPanel_K3.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    break;
                case 3:
                    Model_Image.Source = new BitmapImage(new Uri("/Resources/Page1_Vol2Pics/Схема САР-4.jpg", UriKind.Relative));
                    CollapseAllVariables();
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_K2.Visibility = Visibility.Visible;
                    StackPanel_K3.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_si.Visibility = Visibility.Visible;

                    break;
                case 4:
                    Model_Image.Source = new BitmapImage(new Uri("/Resources/Page1_Vol2Pics/Схема САР-5.jpg", UriKind.Relative));
                    CollapseAllVariables();
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_K2.Visibility = Visibility.Visible;
                    StackPanel_K3.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            this.modelNumber = modelNumber;
        }
        private void CollapseAllVariables()
        {
            StackPanel_K1.Visibility = Visibility.Collapsed;
            StackPanel_K2.Visibility = Visibility.Collapsed;
            StackPanel_K3.Visibility = Visibility.Collapsed;
            StackPanel_si.Visibility = Visibility.Collapsed;
            StackPanel_T1.Visibility = Visibility.Collapsed;
            StackPanel_T2.Visibility = Visibility.Collapsed;
            StackPanel_Tnu.Visibility = Visibility.Collapsed;
        }
    }
}
