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
using OxyPlot.Wpf;

namespace TAU_Complex.Pages.Page1
{
    public partial class Page1_Main : Page
    {
        private class Combo
        {
            public string name { get; set; }
            public Page page { get; set; }
        }

        public Page1_Main()
        {
            InitializeComponent();
            List<Combo> list = new List<Combo>
            {
                new Combo { name = "Идеальное усилительное (безынерционное) звено", page = new Page1_1(plot) },
                new Combo { name = "Апериодическое (инерционное) звено", page = new Page1_2() },
                new Combo { name = "Апериодическое звено второго порядка", page = new Page1_3() },
                new Combo { name = "Колебательное звено", page = new Page1_4() },
                new Combo { name = "Идеальное интегрирующее звено", page = new Page1_5() },
                new Combo { name = "Инерционное (реальное) интегрирующие звено", page = new Page1_6() },
                new Combo { name = "Инерционное дифференцирующие звено", page = new Page1_7() }
            };
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
            workTable.Content = (comboBox.SelectedItem as Combo).page;
            PlotModel switchedModel = ((comboBox.SelectedItem as Combo).page as SubPageModule1)?.thisModel;
            if (switchedModel != null)
            {
                plot.Model = switchedModel;
            }          
            
        }
    }
    interface SubPageModule1
    {
        PlotModel thisModel { get; set; }
        PlotView plotView { get; set; }
    }
}
