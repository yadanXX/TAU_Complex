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
using TAU_Complex.Pages.Vol1.Page1;
using TAU_Complex.Pages.Vol1;
using TAU_Complex.Pages.Vol2;

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
        private Page page7_Vol1 = new Page7_Vol1();
        private Page Page1_Vol2 = new Page1_Vol2();
        private Page Page2_Vol2 = new Page2_Vol2();
        private Page Page3_Vol2 = new Page3_Vol2();
        private Page Page4_Vol2 = new Page4_Vol2();
        private Page Page5_Vol2 = new Page5_Vol2();
        private Page Page6_Vol2 = new Page6_Vol2();

        public MainWindow()
        {
            InitializeComponent();
            MainPanel.Content = pageMainImage;
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "Button_1_FirstVol":
                    MainPanel.Content = page1;
                    break;
                case "Button_2_FirstVol":
                    MainPanel.Content = page2;
                    break;
                case "Button_3_FirstVol":
                    MainPanel.Content = page3;
                    break;
                case "Button_4_FirstVol":
                    MainPanel.Content = page4;
                    break;
                case "Button_5_FirstVol":
                    MainPanel.Content = page5;
                    break;
                case "Button_6_FirstVol":
                    MainPanel.Content = page6;
                    break;
                case "Button_7_FirstVol":
                    MainPanel.Content = page7_Vol1;
                    break;
                case "Button_1_SecondVol":
                    MainPanel.Content = Page1_Vol2;
                    break;
                case "Button_2_SecondVol":
                    MainPanel.Content = Page2_Vol2;
                    break;
                case "Button_3_SecondVol":
                    MainPanel.Content = Page3_Vol2;
                    break;
                case "Button_4_SecondVol":
                    MainPanel.Content = Page4_Vol2;
                    break;
                case "Button_5_SecondVol":
                    MainPanel.Content = Page5_Vol2;
                    break;
                case "Button_6_SecondVol":
                    MainPanel.Content = Page6_Vol2;
                    break;
            }
            SideMenu_Button.Visibility = Visibility.Visible;
            Options_Button.Visibility = Visibility.Visible;
        }
        private void Button_Choose_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "Button_Choose_FirstVol":
                    FirstVol_StackPanel.Visibility = Visibility.Visible;
                    SecondVol_StackPanel.Visibility = Visibility.Collapsed;
                    break;
                case "Button_Choose_SecondVol":
                    SecondVol_StackPanel.Visibility = Visibility.Visible;
                    FirstVol_StackPanel.Visibility = Visibility.Collapsed;
                    break;

            }
        }

        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.ShowDialog();
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
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
            SideMenu_Button.Visibility = Visibility.Collapsed;
            Options_Button.Visibility = Visibility.Collapsed;
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

    }
}
