using System;

using UWPLockStep.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views
{
    public sealed partial class TabbedPage : Page
    {
        public TabbedViewModel ViewModel { get; } = new TabbedViewModel();

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
