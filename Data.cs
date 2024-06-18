using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;

namespace TAU_Complex
{
    internal static class Data
    {
        public enum Modes
        {
            First = 0,
            Second = 1,
            Third = 2
        }

        public static Modes ChosedMode = Modes.Third;

        public static double Dt = 0.001;

        public static double GetDt(List<double> TList, double tk)
        {
            double resultDt = 0;
            switch (ChosedMode)
            {
                case Modes.First:
                    resultDt = tk / 10000;
                    break;
                case Modes.Second:
                    double minT = TList.Where(T => T > 0).Min();
                    resultDt = minT / 100;
                    break;
                case Modes.Third:
                    resultDt = Dt;
                    break;
            }
            Dt = resultDt;
            return resultDt;
        }

    }
}
