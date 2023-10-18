using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace TAU_Complex
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainPanel.Content = new PageMainImage();
        }

        #region Кнопки закрытия и перетаскивания
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        //private void Border_MouseDown_2(object sender, MouseButtonEventArgs e)
        //{
        //    this.Close();            
        //}
        //private void Border_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Border_close_Copy.Background = System.Windows.Media.Brushes.Red;
        //}
        //private void Border_close_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Border_close_Copy.Background = brush2;
        //}
        #endregion
    }
}
