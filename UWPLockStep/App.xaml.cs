using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using UWPLockStep.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWPLockStep
{
    public sealed partial class App : Application
    {
    //    private Lazy<ActivationService> _activationService;

    //    private ActivationService ActivationService
    //    {
    //        get { return _activationService.Value; }
    //    }
        public App() => InitializeComponent();

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated != false) return;
            if (rootFrame.Content == null) rootFrame.Navigate(typeof(Views.MainView), e.Arguments);
            Window.Current.Activate();
        }

        private static void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        //public App()
        //{
        //   // InitializeComponent();
        //    Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

        //    // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
        //    _activationService = new Lazy<ActivationService>(CreateActivationService);
        //}

        //protected override async void OnLaunched(LaunchActivatedEventArgs args)
        //{
        //    if (!args.PrelaunchActivated)
        //    {
        //        await ActivationService.ActivateAsync(args);
        //    }
        //}

        //protected override async void OnActivated(IActivatedEventArgs args)
        //{
        //    await ActivationService.ActivateAsync(args);
        //}

        //private ActivationService CreateActivationService()
        //{
        //    return new ActivationService(this, typeof(Views.MainView), new Lazy<UIElement>(CreateShell));
        //}

        //private UIElement CreateShell()
        //{
        //    return new Views.MainView();
        //}
    }
}
