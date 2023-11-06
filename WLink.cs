using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAU_Complex
{
    internal class WLink
    {
        public static double NonEnertion(double xv, double k)
        { //Безынерционное звено (П - регулятор).
            return xv * k;
        }
        public static double IdealInter(double xv, double k, double x1, double Dt)
        {
            /* Идеальное интегрирующее звено (И - регулятор)
             * xv - вход
             * k - коэф усиления
             * x1 - предыдущий выход1(промежуточный), после изменения текущий1
             * Dt - дельта t
             */
            return x1 + xv * Dt * k;
        }
        public static (double, double, double) Integrating(double xv, double k, double T, double x1, double x2, double Dt)
        {
            /* Интегрирующее звено с замедлением.
             * xv - вход
             * k - коэф усиления
             * T - постоянная времени
             * x1 - предыдущий выход1(промежуточный), после изменения текущий1
             * x2 - предыдущий выход2(промежуточный), после изменения текущий2 x'
             * Dt - дельта t
             * x3 - выход1'' (промежуточный производный)
             * x - выход
             */
            double x, x3;
            x3 = xv - x2;
            x2 = x2 + (x3 * Dt) / T;
            x1 = x1 + x2 * Dt;
            x = k * x1;
            return (x, x1, x2);
        }
        public static (double, double) Aperiodic(double xv, double k, double T, double x1, double Dt)
        {
            /* Апериодическое звено 1 - го порядка.
             * xv - вход
             * k - коэф усиления
             * T - постоянная времени
             * x1 - предыдущий выход1(промежуточный), после изменения текущий1
             * Dt - дельта t
             * x2 - выход1' (промежуточный производный)
             * x - выход
             */
            double x, x2;
            x2 = (xv - x1) / T;
            x1 = (x1 + Dt * x2);
            x = k * x1;
            return (x, x1);
        }
        public static (double, double) Exodrom(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            /* Изодромное звено (ПИ - регулятор).
             * xv - вход
             * k - коэф усиления
             * T0,T1 - постоянная времени
             * xi1 - предыдущий выход1(промежуточный), после изменения текущий1
             * Dt - дельта t
             * x - выход
             */
            double x;
            xi1 = xi1 + xv * Dt / T1;
            x = xi1 + xv * T0 / T1 * k;
            return (x, xi1);
        }
        public static (double, double, double) Oscillatory(double xv, double k, double T0, double T1, double x1, double xi1, double Dt)
        {
            /* Колебательное звено
             * xv - вход
             * k - коэф усиления
             * T0,T1 - постоянная времени
             * x1 - предыдущий выход1(промежуточный), после изменения текущий1
             * xi1 - предыдущий выход1'(промежуточно производный),после изменения текущий1'
             * Dt - дельта t
             * x - выход
             */
            xi1 = xi1 + (xv - T1 * xi1 - x1) * Dt / Math.Pow(T0, 2);
            x1 = x1 + xi1 * Dt;
            return (k * x1, x1, xi1);
        }
        public static (double, double) Difdelay(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            /* Дифференцирующее звено с замедлением.
             * xv - вход
             * k - коэф усиления
             * T0,T1 - постоянная времени
             * xi1 - предыдущий выход1(промежуточный), после изменения текущий1
             * Dt - дельта t
             * x - выход
             */
            double x;
            x = (xv - xi1) / T0 * T1 * k;
            xi1 = xi1 + (xv - xi1) / T0 * Dt;
            return (x, xi1);
        }
        public static (double, double) Dif(double xv, double k, double xi1, double Dt)
        {
            /* 
             * Идеальное дифференцирующее звено
                 * xv - вход
                 * k - коэф усиления
                 * xi1 - предыдущий вход1(промежуточный), после изменения текущий1
                 * Dt - дельта t
                 * x - выход
                 */
            double x;
            x = (xv - xi1) * k / Dt;
            xi1 = xv;
            return (x, xi1);
        }
        public static (double, double) PropDifDelay(double xv, double k, double T0, double T1, double xi1, double Dt)
        {
            /* Пропорционально-дифференциальный регулятор с замедлением.
                 * xv - вход
                 * k - коэф усиления
                 * T0,T1 - постоянная времени
                 * xi1 - предыдущий выход1(промежуточный), после изменения текущий1
                 * xi1d1 - выход 1 в первой производой
                 * Dt - дельта t
                 * x - выход
                 */
            double x, xi1d1;
            xi1d1 = (xv - xi1) / T0;
            xi1 = xi1 + xi1d1 * Dt;
            x = k * (xi1 + xi1d1 * T1);
            return (x, xi1);
        }
        public static (double, double, double, double) Mod(double xv, double k, double a1, double a2, double a3, double xid2, double xid1, double xid, double Dt)
        {
            /* Кастомное звено третьго порядка k/s3+a1s2+a2s+a3
                             * xv - вход
                             * k - коэф усиления
                             * a1,a2,a3 - постоянная времени
                             * xid2,xid1,xid - предыдущий выход1(промежуточный), после изменения текущий1
                             * xi3, xi2, xi1, xi - текущие(промежуточные)
                             * Dt - дельта t
                             * x - выход
                             */
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
