﻿using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TAU_Complex.Pages.Vol1.Page1
{
    /// <summary>
    /// Логика взаимодействия для Page1_1.xaml
    /// </summary>
    public partial class Page1_1 : Page, SubPageModule1
    {
        public Page1_1(PlotView plot)
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
            double k1, tk;
            try
            {
                k1 = Convert.ToDouble(textBoxk1.Text.Replace(".", ","));
                tk = Convert.ToDouble(textBoxTk.Text.Replace(".", ","));
                if (tk <= 0 || k1 <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                return;
            }

            double Dt = Data.GetDt(null,tk);

            List<DataPoint> dataPoints = new List<DataPoint>();
            for (double i = 0; i < tk; i += Dt)
            {
                dataPoints.Add(new DataPoint(i, k1));
            }
            plotView.Model = Utils.GetLinearPlotModel("График переходной характеристики", dataPoints, "t", "Qвых(t)");
            thisModel = plotView.Model;
        }
    }

}
