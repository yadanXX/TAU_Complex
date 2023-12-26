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
using TAU_Complex.Pages.Page1;
using TAU_Complex.Pages;

namespace TAU_Complex
{

    public partial class MainWindow : Window
    {
        public Page ActivePage;
        private Page pageMainImage = new PageMainImage();
        private Page page1 = new Page1_Main();
        private Page page2 = new Page2();
        private Page page3 = new Page3();
        private Page page4 = new Page4();
        private Page page5 = new Page5();
        private Page page6 = new Page6();

        public MainWindow()
        {
            InitializeComponent();
            MainPanel.Content = pageMainImage;
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "button1":
                    MainPanel.Content = page1;
                    break;
                case "button2":
                    MainPanel.Content = page2;
                    break;
                case "button3":
                    MainPanel.Content = page3;
                    break;
                case "button4":
                    MainPanel.Content = page4;
                    break;
                case "button5":
                    MainPanel.Content = page5;
                    break;
                case "button6":
                    MainPanel.Content = page6;
                    break;

            }
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

        private void Button_Click_min(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Button_Click_exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_close(object sender, RoutedEventArgs e)
        {
            MainPanel.Content = pageMainImage;
        }
        private void Button_Click_slide(object sender, RoutedEventArgs e)
        {
            if (slideCol.Width.Value != 0)
            {
                slideCol.Width = new GridLength(0);
            }
            else
            {
                slideCol.Width = new GridLength(200);
            }

        }
        #endregion


    }
}
