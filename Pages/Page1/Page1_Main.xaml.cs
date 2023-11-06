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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TAU_Complex.Pages.Page1
{
    public partial class Page1_Main : Page
    {
        private class Combo
        {
            public string name { get; set; }
            public Page pageName { get; set; }
        }

        public Page1_Main()
        {
            InitializeComponent();
            List<Combo> list = new List<Combo>();
            list.Add(new Combo { name = "Идеальное усилительное (безынерционное) звено", pageName = new Page1_1(plot) });
            list.Add(new Combo { name = "Апериодическое (инерционное) звено", pageName = new Page1_2() });
            list.Add(new Combo { name = "Апериодическое звено второго порядка", pageName = new Page1_3() });
            list.Add(new Combo { name = "Колебательное звено", pageName = new Page1_4() });
            list.Add(new Combo { name = "Идеальное интегрирующее звено", pageName = new Page1_5() });
            list.Add(new Combo { name = "Инерционное (реальное) интегрирующие звено", pageName = new Page1_6() });
            list.Add(new Combo { name = "Инерционное дифференцирующие звено", pageName = new Page1_7() });
            comboBox.ItemsSource = list;
            comboBox.DisplayMemberPath = "name";
            OpenPage(comboBox);

            plot.Model = Utils.GetLinearPlotModel("График переходной характеристики", null, "t", "Qвых(t)");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenPage(comboBox);
        }
        private void OpenPage(ComboBox comboBox)
        {
            workTable.Content = (comboBox.SelectedItem as Combo).pageName;
        }
    }
}
