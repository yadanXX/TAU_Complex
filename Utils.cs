using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
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

        public static PlotModel GetLgPlotModel(string title, List<DataPoint> listPoints1, List<DataPoint> listPoints2, string XTitle, string YTitle)
        {
            PlotModel MyModel = new PlotModel();
            MyModel.Title = title;
            var line1 = new OxyPlot.Series.LineSeries();
            var line2 = new OxyPlot.Series.LineSeries();
            if (listPoints1 == null) listPoints1 = new List<DataPoint>() { new DataPoint(0, 0) };
            if (listPoints2 == null) listPoints2 = new List<DataPoint>() { new DataPoint(0, 0) };
            line1.Points.AddRange(listPoints1);
            line2.Points.AddRange(listPoints2);
            MyModel.Series.Add(line1);
            MyModel.Series.Add(line2);

            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineThickness = 1,
                Title = YTitle
            });

            MyModel.Axes.Add(new LogarithmicAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dash,
                MinorGridlineThickness = 1,
                Title = XTitle
            });

            return MyModel;
        }
        public static PlotModel GetRootPlotModel(string title, List<DataPoint> listPoints1, List<DataPoint> listPoints2, string XTitle, string YTitle)
        {
            PlotModel MyModel = new PlotModel();
            MyModel.Title = title;
            MyModel.Legends.Add(new Legend() { LegendPosition = LegendPosition.RightTop, Key = "Ноль", LegendPlacement = LegendPlacement.Outside });
            MyModel.Legends.Add(new Legend() { LegendPosition = LegendPosition.RightMiddle, Key = "Полюс", LegendPlacement = LegendPlacement.Outside });
            ScatterSeries line1 = new ScatterSeries() { MarkerType = MarkerType.Triangle, MarkerSize = 10, Title = "Ноль", LegendKey = "Ноль" };
            ScatterSeries line2 = new ScatterSeries() { MarkerType = MarkerType.Diamond, MarkerSize = 7, Title = "Полюс", LegendKey = "Полюс" };
            if (listPoints1 == null) listPoints1 = new List<DataPoint>() { new DataPoint(0, 0) };
            if (listPoints2 == null) listPoints2 = new List<DataPoint>() { new DataPoint(0, 0) };
            foreach (DataPoint point in listPoints1)
            {
                line1.Points.Add(new ScatterPoint(point.X, point.Y, double.NaN, 1));
            }
            foreach (DataPoint point in listPoints2)
            {
                line2.Points.Add(new ScatterPoint(point.X, point.Y, double.NaN, 10));
            }
            MyModel.Series.Add(line1);
            MyModel.Series.Add(line2);

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

    }
}
