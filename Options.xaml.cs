using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace TAU_Complex
{
    /// <summary>
    /// Логика взаимодействия для Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            textBoxDt.Text = Data.Dt.ToString();
            switch (Data.ChosedMode)
            {
                case Data.Modes.First:
                    FirstmMode_RadioButton.IsChecked = true;
                    break;

                case Data.Modes.Second:
                    SecondMode_RadioButton.IsChecked = true;
                    break;

                case Data.Modes.Third:
                    ThridMode_RadioButton.IsChecked = true;
                    break;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (FirstmMode_RadioButton == null ||
                SecondMode_RadioButton == null ||
                ThridMode_RadioButton == null ||
                textBoxDt == null)
            {
                return;
            }
            if ((bool)FirstmMode_RadioButton.IsChecked)
            {
                Data.ChosedMode = Data.Modes.First;
                textBoxDt.IsEnabled = false;
            }
            else if ((bool)SecondMode_RadioButton.IsChecked)
            {
                Data.ChosedMode = Data.Modes.Second;
                textBoxDt.IsEnabled = false;
            }
            else if ((bool)ThridMode_RadioButton.IsChecked)
            {
                textBoxDt.IsEnabled = true;
                Data.ChosedMode = Data.Modes.Third;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            double Dt;
            try
            {
                Dt = Convert.ToDouble(textBoxDt.Text.Replace(".", ","));
                if (Dt <= 0) throw new Exception();
            }
            catch (Exception)
            {
                ErrorWindow f = new ErrorWindow();
                f.ShowDialog();
                e.Cancel = true;
                return;
            }
            Data.Dt = Dt;           
        }
    }
}
