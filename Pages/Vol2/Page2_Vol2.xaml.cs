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

namespace TAU_Complex.Pages.Vol2
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2_Vol2 : Page
    {
        private int modelNumber = 1;
        private bool isFilter = false;
        public Page2_Vol2()
        {
            InitializeComponent();
            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
            RadioButton_Checked(null, null);
            Filter_CheckBox_Checked(null, null);
            OutRage_ComboBox_SelectionChanged(null, null);
        }

        delegate (double, double) Filter(double xv, double K, double T, double x1, double Dt);
        private static Random rnd = new Random();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (plotView1.Model.Series.FirstOrDefault() as LineSeries).Points.Clear();
            plotView1.InvalidatePlot(true);
            double K = 0, K1 = 0, T = 0, T1 = 0, T2 = 0, tk = 0, Tnu = 0, si = 0, si1 = 0, Kf = 0, Tf = 0;
            try
            {
                if (StackPanel_K.Visibility == Visibility.Visible)
                {
                    K = Convert.ToDouble(textBoxK.Text.Replace(".", ","));
                    if (K <= 0) throw new Exception();
                }
                if (StackPanel_K1.Visibility == Visibility.Visible)
                {
                    K1 = Convert.ToDouble(textBoxK1.Text.Replace(".", ","));
                    if (K1 <= 0) throw new Exception();
                }
                if (StackPanel_si.Visibility == Visibility.Visible)
                {
                    si = Convert.ToDouble(textBoxsi.Text.Replace(".", ","));
                    if (si <= 0) throw new Exception();
                }
                if (StackPanel_si1.Visibility == Visibility.Visible)
                {
                    si1 = Convert.ToDouble(textBoxsi1.Text.Replace(".", ","));
                    if (si1 <= 0) throw new Exception();
                }
                if (StackPanel_T.Visibility == Visibility.Visible)
                {
                    T = Convert.ToDouble(textBoxT.Text.Replace(".", ","));
                    if (T <= 0) throw new Exception();
                }
                if (StackPanel_T1.Visibility == Visibility.Visible)
                {
                    T1 = Convert.ToDouble(textBoxT1.Text.Replace(".", ","));
                    if (T1 <= 0) throw new Exception();
                }
                if (StackPanel_Tnu.Visibility == Visibility.Visible)
                {
                    Tnu = Convert.ToDouble(textBoxTnu.Text.Replace(".", ","));
                    if (Tnu <= 0) throw new Exception();
                }
                if (StackPanel_T2.Visibility == Visibility.Visible)
                {
                    T2 = Convert.ToDouble(textBoxT2.Text.Replace(".", ","));
                    if (T2 <= 0) throw new Exception();
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

            Filter filter = delegate (double xvDF, double kDf, double TDf, double x1DF, double DtDf) // анонимное объявление функции для глобалтно определённого делагата Filter
            {
                return (xv, 0);
            };

            double wvf, tempf = 0;
            if ((bool)Filter_CheckBox.IsChecked)
            {
                filter = WLink.Aperiodic; // переопределение делегата на другую функцию

                try
                {
                    Kf = Convert.ToDouble(textBoxKF.Text.Replace(".", ","));
                    Tf = Convert.ToDouble(textBoxTF.Text.Replace(".", ","));
                    if (Kf <= 0 || Tf <= 0) throw new Exception();
                }
                catch (Exception)
                {
                    ErrorWindow f = new ErrorWindow();
                    f.ShowDialog();
                    return;
                }
            }


            double wv1, wv2 = 0, temp1 = 0, temp2 = 0;
            double temp111 = 0, temp112 = 0, temp121 = 0, temp122 = 0, temp131 = 0, temp132 = 0, temp21 = 0, temp22 = 0;
            double wv111, wv112, wv121, wv122, wv131, wv132, wv2pos = 0;
            double invTime, amplit, freq, pointCounter, mathEx, disper;
            switch (modelNumber)
            {
                case 0:


                    for (double i = 0; i < tk; i += Dt)
                    {
                        (wvf, tempf) = filter(xv, Kf, Tf, tempf, Dt);
                        (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);
                        (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                        dataPoints1.Add(new DataPoint(i, wv2));
                    }
                    break;
                case 1:

                    for (double i = 0; i < tk; i += Dt)
                    {
                        (wvf, tempf) = filter(xv, Kf, Tf, tempf, Dt);

                        (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                        (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                        (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                        (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                        (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                        (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                        (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                        dataPoints1.Add(new DataPoint(i, wv2));

                    }
                    break;
                case 2:
                    switch (OutRage_ComboBox.SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {

                                (wvf, tempf) = filter(xv - outRageStep(amplit, invTime, i), Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);
                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2 += outRageStep(amplit, invTime, i);
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 1:

                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                freq = Convert.ToDouble(Frequency_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0 || freq <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {

                                (wvf, tempf) = filter(xv - outRageSin(amplit, freq, i, invTime), Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);
                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2 += outRageSin(amplit, freq, i, invTime);
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 2:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                pointCounter = Convert.ToDouble(PointCount_TextBox.Text.Replace(".", ","));
                                mathEx = Convert.ToDouble(Expect_TextBox.Text.Replace(".", ","));
                                disper = Convert.ToDouble(Dispertion_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || pointCounter <= 0 || disper <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            double outRageRand = 0;
                            double randFreq = (tk - invTime) / pointCounter;
                            double randCurStage = invTime;

                            for (double i = 0; i < tk; i += Dt)
                            {
                                if (i >= randCurStage)
                                {
                                    outRageRand = NextGaussian(mathEx, disper);
                                    randCurStage += randFreq;
                                }

                                (wvf, tempf) = filter(xv - outRageRand, Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);
                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2 += outRageRand;
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (OutRage_ComboBox.SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {
                                (wvf, tempf) = filter(xv - outRageStep(amplit, invTime, i), Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2 += outRageStep(amplit, invTime, i);

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 1:

                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                freq = Convert.ToDouble(Frequency_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0 || freq <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {
                                (wvf, tempf) = filter(xv - outRageSin(amplit, freq, i, invTime), Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2 += outRageSin(amplit, freq, i, invTime);

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 2:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                pointCounter = Convert.ToDouble(PointCount_TextBox.Text.Replace(".", ","));
                                mathEx = Convert.ToDouble(Expect_TextBox.Text.Replace(".", ","));
                                disper = Convert.ToDouble(Dispertion_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || pointCounter <= 0 || disper <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            double outRageRand = 0;
                            double randFreq = (tk - invTime) / pointCounter;
                            double randCurStage = invTime;

                            for (double i = 0; i < tk; i += Dt)
                            {
                                if (i >= randCurStage)
                                {
                                    outRageRand = NextGaussian(mathEx, disper);
                                    randCurStage += randFreq;
                                }

                                (wvf, tempf) = filter(xv - outRageRand, Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2 += outRageRand;

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;
                    }
                    break;
                case 4:
                    switch (OutRage_ComboBox.SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {
                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);

                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2pos = wv2;
                                wv2 += outRageStep(amplit, invTime, i);
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 1:

                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                freq = Convert.ToDouble(Frequency_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0 || freq <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {

                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);

                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2pos = wv2;
                                wv2 += outRageSin(amplit, freq, i, invTime);
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 2:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                pointCounter = Convert.ToDouble(PointCount_TextBox.Text.Replace(".", ","));
                                mathEx = Convert.ToDouble(Expect_TextBox.Text.Replace(".", ","));
                                disper = Convert.ToDouble(Dispertion_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || pointCounter <= 0 || disper <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            double outRageRand = 0;
                            double randFreq = (tk - invTime) / pointCounter;
                            double randCurStage = invTime;

                            for (double i = 0; i < tk; i += Dt)
                            {
                                if (i >= randCurStage)
                                {
                                    outRageRand = NextGaussian(mathEx, disper);
                                    randCurStage += randFreq;
                                }


                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);
                                (wv1, temp1) = WLink.PropDifDelay(wvf, 1 / K1, Tnu, T1, temp1, Dt);

                                (wv2, temp2) = WLink.Aperiodic(wv1, K, T, temp2, Dt);
                                wv2pos = wv2;
                                wv2 += outRageRand;
                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;
                    }
                    break;
                case 5:
                    switch (OutRage_ComboBox.SelectedIndex)
                    {
                        case 0:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {
                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2pos = wv2;

                                wv2 += outRageStep(amplit, invTime, i);

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 1:

                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                amplit = Convert.ToDouble(Amplit_TextBox.Text.Replace(".", ","));
                                freq = Convert.ToDouble(Frequency_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || amplit <= 0 || freq <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            for (double i = 0; i < tk; i += Dt)
                            {
                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2pos = wv2;

                                wv2 += outRageSin(amplit, freq, i, invTime);

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;

                        case 2:
                            try
                            {
                                invTime = Convert.ToDouble(InvTime_TextBox.Text.Replace(".", ","));
                                pointCounter = Convert.ToDouble(PointCount_TextBox.Text.Replace(".", ","));
                                mathEx = Convert.ToDouble(Expect_TextBox.Text.Replace(".", ","));
                                disper = Convert.ToDouble(Dispertion_TextBox.Text.Replace(".", ","));
                                if (invTime <= 0 || pointCounter <= 0 || disper <= 0) throw new Exception();
                            }
                            catch (Exception)
                            {
                                ErrorWindow f = new ErrorWindow();
                                f.ShowDialog();
                                return;
                            }

                            double outRageRand = 0;
                            double randFreq = (tk - invTime) / pointCounter;
                            double randCurStage = invTime;

                            for (double i = 0; i < tk; i += Dt)
                            {
                                if (i >= randCurStage)
                                {
                                    outRageRand = NextGaussian(mathEx, disper);
                                    randCurStage += randFreq;
                                }

                                (wvf, tempf) = filter(xv + wv2pos - wv2, Kf, Tf, tempf, Dt);

                                (wv111, temp111) = WLink.Difdelay(wvf, 1 / K1, Tnu, T1, temp111, Dt);
                                (wv112, temp112) = WLink.Difdelay(wv111, 1, Tnu, T1, temp112, Dt);

                                (wv121, temp121) = WLink.Difdelay(wvf, si1, Tnu, T1, temp121, Dt);
                                (wv122, temp122) = WLink.Aperiodic(wv121, 1 / K1, Tnu, temp122, Dt);

                                (wv131, temp131) = WLink.Aperiodic(wvf, 1 / K1, Tnu, temp131, Dt);
                                (wv132, temp132) = WLink.Aperiodic(wv131, 1, Tnu, temp132, Dt);

                                (wv2, temp21, temp22) = WLink.Oscillatory(wv112 + wv122 + wv132, K, T, 2 * T * si, temp21, temp22, Dt);

                                wv2pos = wv2;

                                wv2 += outRageRand;

                                dataPoints1.Add(new DataPoint(i, wv2));
                            }
                            break;
                    }
                    break;
            }
            //legend1 = $"K1={textBoxK1.Text} K2={textBoxK2.Text} K3={textBoxK3.Text} T1={textBoxT1.Text} T2={textBoxT2.Text} tk={textBoxtk.Text}";

            plotView1.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints1, "t", "Qвых(t)");

        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (Raz_RadioButton == null ||
                Voz_RadioButton == null ||
                Otk_RadioButton == null ||
                Pic_Radiobuttom_1 == null ||
                Pic_Radiobuttom_2 == null)
            {
                return;
            }
            if ((bool)Raz_RadioButton.IsChecked)
            {
                if ((bool)Pic_Radiobuttom_1.IsChecked)
                {
                    SetModel(0);
                }
                else if ((bool)Pic_Radiobuttom_2.IsChecked)
                {
                    SetModel(1);
                }
            }
            if ((bool)Voz_RadioButton.IsChecked)
            {
                if ((bool)Pic_Radiobuttom_1.IsChecked)
                {
                    SetModel(2);
                }
                else if ((bool)Pic_Radiobuttom_2.IsChecked)
                {
                    SetModel(3);
                }
            }
            if ((bool)Otk_RadioButton.IsChecked)
            {
                if ((bool)Pic_Radiobuttom_1.IsChecked)
                {
                    SetModel(4);
                }
                else if ((bool)Pic_Radiobuttom_2.IsChecked)
                {
                    SetModel(5);
                }
            }
        }
        private void SetModel(int modelNumber)
        {
            switch (modelNumber)
            {
                case 0:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/разомкнутое апериодическое с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/разомкнутое апериодическое.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    break;
                case 1:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/разомкнутое колебательное с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/разомкнутое колебательное.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    StackPanel_si.Visibility = Visibility.Visible;
                    StackPanel_si1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по возмущению апериодическое с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по возмущению апериодическое.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    StackPanel_Random.Visibility = Visibility.Visible;
                    break;
                case 3:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по возмущению колебательное с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по возмущению колебательное.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    StackPanel_si.Visibility = Visibility.Visible;
                    StackPanel_si1.Visibility = Visibility.Visible;
                    StackPanel_Random.Visibility = Visibility.Visible;
                    break;
                case 4:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по отклонению апериодическое с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по отклонению апериодическое.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    StackPanel_Random.Visibility = Visibility.Visible;
                    break;
                case 5:
                    if (isFilter)
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по отклонению колебательное с фильтром.png", UriKind.Relative));
                    }
                    else
                    {
                        Model_Image.Source = new BitmapImage(new Uri("/Resources/Vol2/Page2_Vol2Pics/по отклонению колебательное.png", UriKind.Relative));
                    }
                    CollapseAllVariables();
                    StackPanel_K.Visibility = Visibility.Visible;
                    StackPanel_K1.Visibility = Visibility.Visible;
                    StackPanel_T.Visibility = Visibility.Visible;
                    StackPanel_T1.Visibility = Visibility.Visible;
                    StackPanel_Tnu.Visibility = Visibility.Visible;
                    StackPanel_si.Visibility = Visibility.Visible;
                    StackPanel_si1.Visibility = Visibility.Visible;
                    StackPanel_Random.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
            this.modelNumber = modelNumber;
        }
        private void CollapseAllVariables()
        {
            StackPanel_K.Visibility = Visibility.Collapsed;
            StackPanel_K1.Visibility = Visibility.Collapsed;
            StackPanel_si.Visibility = Visibility.Collapsed;
            StackPanel_si1.Visibility = Visibility.Collapsed;
            StackPanel_T.Visibility = Visibility.Collapsed;
            StackPanel_T1.Visibility = Visibility.Collapsed;
            StackPanel_T2.Visibility = Visibility.Collapsed;
            StackPanel_Tnu.Visibility = Visibility.Collapsed;
            StackPanel_Random.Visibility = Visibility.Collapsed;
        }
        private void Filter_CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if ((bool)Filter_CheckBox.IsChecked)
            {
                StackPanel_FilterKT.Visibility = Visibility.Visible;
                isFilter = true;
                SetModel(modelNumber);
            }
            else
            {
                StackPanel_FilterKT.Visibility = Visibility.Hidden;
                isFilter = false;
                SetModel(modelNumber);
            }

        }
        private double outRageStep(double amplit, double invTime, double time)
        {
            if (time >= invTime)
            {
                return amplit;
            }
            else return 0;
        }
        private double outRageSin(double amplit, double frequancy, double time, double invTime)
        {
            if (time >= invTime)
            {
                return amplit * Math.Sin(frequancy * time * 2.0 * Math.PI - invTime * 2.0 * Math.PI);
            }
            else return 0;
        }
        private static double NextGaussian(double mu, double sigma)
        {
            // рандом по нормальному закону 
            // mu - пик
            // sigma - разброс
            double rand_normal;
            // do
            // {
            //double mu = 0.5;
            // double sigma = mu / 3;
            var u1 = rnd.NextDouble();
            var u2 = rnd.NextDouble();
            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            rand_normal = mu + sigma * rand_std_normal;
            //} while (rand_normal < 0 || rand_normal > 1);
            return Math.Round(rand_normal, 1);
        }

        private void OutRage_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Amplit_DockPanel == null ||
            Dispertion_DockPanel == null ||
            Expect_DockPanel == null ||
            Frequency_DockPanel == null ||
            InvTime_DockPanel == null ||
            PointCount_DockPanel == null)
            {
                return;
            }
            switch (OutRage_ComboBox.SelectedIndex)
            {
                case 0:
                    CollapseAllOutRage();
                    Amplit_DockPanel.Visibility = Visibility.Visible;
                    InvTime_DockPanel.Visibility = Visibility.Visible;
                    break;
                case 1:
                    CollapseAllOutRage();
                    Amplit_DockPanel.Visibility = Visibility.Visible;
                    InvTime_DockPanel.Visibility = Visibility.Visible;
                    Frequency_DockPanel.Visibility = Visibility.Visible;
                    break;
                case 2:
                    CollapseAllOutRage();
                    Dispertion_DockPanel.Visibility = Visibility.Visible;
                    Expect_DockPanel.Visibility = Visibility.Visible;
                    InvTime_DockPanel.Visibility = Visibility.Visible;
                    PointCount_DockPanel.Visibility = Visibility.Visible;
                    break;

            }
        }
        private void CollapseAllOutRage()
        {
            Amplit_DockPanel.Visibility = Visibility.Collapsed;
            Dispertion_DockPanel.Visibility = Visibility.Collapsed;
            Expect_DockPanel.Visibility = Visibility.Collapsed;
            Frequency_DockPanel.Visibility = Visibility.Collapsed;
            InvTime_DockPanel.Visibility = Visibility.Collapsed;
            PointCount_DockPanel.Visibility = Visibility.Collapsed;
        }
    }
}
