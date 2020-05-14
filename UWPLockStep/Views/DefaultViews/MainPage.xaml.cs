using System;

using UWPLockStep.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
