using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ViewModels;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views
{
    public sealed partial class MainView : Page, IViewFor<MainViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(MainViewModel), typeof(MainView), null);

        public MainView()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            this.WhenActivated(disposables => { });

            //var turle = Observable
            //      .FromEventPattern(nextButton, "Click")
            //      .Select(x => (Button)x.Sender);
            //turle.Subscribe(d=> Debug.WriteLine("TURTLEEEEEE"+ turle.ToString()));
        }

        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }

      

    }
}
