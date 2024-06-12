using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAU_Complex
{
    internal class WLink
    {
        /// <summary>
        /// Безынерционное звено (П - регулятор).
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <returns></returns>
        public static double NonEnertion(double xv, double k)
        {
            return xv * k;
        }
        /// <summary>
        /// Идеальное интегрирующее звено (И - регулятор).
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="x1">Предыдущий выход.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static double IdealInter(double xv, double k, double x1, double Dt)
        {
            return x1 + xv * Dt * k;
        }
        /// <summary>
        /// Интегрирующее звено с замедлением.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T">Постоянная времени.</param>
        /// <param name="x1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="x2">Предыдущий выход2(промежуточный), после изменения текущий2 x'.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double, double) Integrating(double xv, double k, double T, double x1, double x2, double Dt)
        {
            double x, x3;
            x3 = xv - x2;
            x2 = x2 + (x3 * Dt) / T;
            x1 = x1 + x2 * Dt;
            x = k * x1;
            return (x, x1, x2);
        }
        /// <summary>
        /// Апериодическое звено 1 - го порядка.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T">Постоянная времени.</param>
        /// <param name="x1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double) Aperiodic(double xv, double k, double T, double x1, double Dt)
        {
            double x, x2;
            x2 = (xv - x1) / T;
            x1 = (x1 + Dt * x2);
            x = k * x1;
            return (x, x1);
        }
        /// <summary>
        /// Изодромное звено (ПИ - регулятор).
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T0">Постоянная времени.</param>
        /// <param name="T1">Постоянная времени.</param>
        /// <param name="xi1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double) Exodrom(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            double x;
            xi1 = xi1 + xv * Dt / T1;
            x = xi1 + xv * T0 / T1 * k;
            return (x, xi1);
        }
        /// <summary>
        /// Колебательное звено.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T0">Постоянная времени.</param>
        /// <param name="T1">Постоянная времени.</param>
        /// <param name="x1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="xi1">Предыдущий выход1'(промежуточно производный),после изменения текущий1'.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double, double) Oscillatory(double xv, double k, double T0, double T1, double x1, double xi1, double Dt)
        {
            xi1 = xi1 + (xv - T1 * xi1 - x1) * Dt / Math.Pow(T0, 2);
            x1 = x1 + xi1 * Dt;
            return (k * x1, x1, xi1);
        }
        /// <summary>
        /// Дифференцирующее звено с замедлением.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T0">Постоянная времени.</param>
        /// <param name="T1">Постоянная времени.</param>
        /// <param name="xi1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double) Difdelay(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            double x;
            x = (xv - xi1) / T0 * T1 * k;
            xi1 = xi1 + (xv - xi1) / T0 * Dt;
            return (x, xi1);
        }
        /// <summary>
        /// Идеальное дифференцирующее звено.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="xi1">Предыдущий вход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double) Dif(double xv, double k, double xi1, double Dt)
        {
            double x;
            x = (xv - xi1) * k / Dt;
            xi1 = xv;
            return (x, xi1);
        }
        /// <summary>
        /// Пропорционально-дифференциальный регулятор с замедлением.
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="T0">Постоянная времени.</param>
        /// <param name="T1">Постоянная времени.</param>
        /// <param name="xi1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double) PropDifDelay(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            double x, xi1d1;
            xi1d1 = (xv - xi1) / T0;
            xi1 = xi1 + xi1d1 * Dt;
            x = k * (xi1 + xi1d1 * T1);
            return (x, xi1);
        }
        /// <summary>
        /// Кастомное звено третьего порядка k/s3+a1s2+a2s+a3
        /// </summary>
        /// <param name="xv">Входной сигнал.</param>
        /// <param name="k">Коэффициент усиления.</param>
        /// <param name="a1">Постоянная времени.</param>
        /// <param name="a2">Постоянная времени.</param>
        /// <param name="a3">Постоянная времени.</param>
        /// <param name="xid2">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="xid1">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="xid">Предыдущий выход1(промежуточный), после изменения текущий1.</param>
        /// <param name="Dt">Шаг дискретизации.</param>
        /// <returns></returns>
        public static (double, double, double, double) Mod(double xv, double k, double a1, double a2, double a3, double xid2, double xid1, double xid, double Dt)
        {
            double x;
            double xi3, xi2, xi1, xi;

            xi3 = xv - a1 * xid2 - a2 * xid1 - a3 * xid;
            xi2 = xid2 + xi3 * Dt;
            xi1 = xid1 + xi2 * Dt;
            xi = xid + xi1 * Dt;
            x = xi * k;
            return (x, xi2, xi1, xi);
        }
    }
}
