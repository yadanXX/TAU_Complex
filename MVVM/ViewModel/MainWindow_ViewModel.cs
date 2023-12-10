using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TAU_Complex.Pages.Page1;
using TAU_Complex.Core;

namespace TAU_Complex.MVVM.ViewModel
{
    internal class MainWindow_ViewModel : ObservableObject
    {
        public RelayCommand HomeImageCommand { get; set; }
        public RelayCommand Mod11MainCommand { get; set; }     
        public HomeImage_ViewModel HomeVM { get; set; }
        public Mod11Main_ViewModel Mod11Main { get; set; }
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindow_ViewModel()
        {
            HomeVM = new HomeImage_ViewModel();
            Mod11Main = new Mod11Main_ViewModel();
            currentView = HomeVM;

            HomeImageCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            Mod11MainCommand = new RelayCommand(o =>
            {
                CurrentView = Mod11Main;
            });
        }
    }
}
