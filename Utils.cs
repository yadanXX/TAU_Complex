using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAU_Complex
{
    public static class Utils
    {
        public static PlotModel GetLinearPlotModel(string title, List<DataPoint> listPoints, string XTitle, string YTitle)
        {
            PlotModel MyModel = new PlotModel();
            MyModel.Title = title;
            var line = new OxyPlot.Series.LineSeries();
            if (listPoints == null) listPoints = new List<DataPoint>() { new DataPoint(0, 0) };
            line.Points.AddRange(listPoints);
            MyModel.Series.Add(line);

            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineThickness = 1,
                Title = YTitle
            });
            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineThickness = 1,
                Title = XTitle
            });

            return MyModel;
        }

        //public static void CenterChart(PlotView plotView)
        //{
        //    double leftMargin = 0;
        //    double topMargin = 0;


        //    foreach (Series item in plotView.Model.Series)
        //    {
        //        LineSeries lineSeries = (item as LineSeries);
                

        //        if (position.X < 0) leftMargin = Math.Max(leftMargin, -position.X);
        //        else topMargin = Math.Max(topMargin, position.Y);
        //    }
        //    // Применяем смещения
        //    plotView.PlotArea.MinViewport.Width += leftMargin * 2;
        //    plotView.PlotArea.MinViewport.Height += topMargin * 2;



        //}

    }
}
