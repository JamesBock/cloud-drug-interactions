using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ViewModels.DisplayObjects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views.DisplayObjects
{
    public abstract class GenericFactorUnitValueView<T> : UserControl, IViewFor<T> where T : FactorUnitValueViewModel
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
    .Register(nameof(ViewModel),
           typeof(T),
           typeof(FactorUnitValueView), null);

        public T ViewModel
        {
            get => (T)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);

        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (T)value;
        }

        public GenericFactorUnitValueView()
        {

            //InitializeComponent();
            //this.WhenActivated(disposableRegistration =>
            //{

            //    //this.OneWayBind(ViewModel,
            //    //    viewModel => viewModel.IsAvailable,
            //    //    view => view.searchResultsListBox.Visibility)
            //    //    .DisposeWith(disposableRegistration);
            //    //this.OneWayBind(ViewModel,
            //    //       viewModel => viewModel.BigDisplayUnit,
            //    //       view => view.factorAmount2.Text)
            //    //       .DisposeWith(disposableRegistration);

            //    this.OneWayBind(ViewModel,
            //            viewModel => viewModel.DisplayString,
            //            view => view.factorAmount.Text)
            //            .DisposeWith(disposableRegistration);

            //    factorAmount.Events().DoubleTapped
            //         .Select(x => Unit.Default)
            //         .InvokeCommand(ViewModel.CycleUnits)
                    // .DisposeWith(disposableRegistration);


                //Observable.FromEventPattern(factorAmount, "DoubleTapped")
                //                                     .Select(x => Unit.Default)
                //                                     //.Throttle(TimeSpan.FromMilliseconds(60d), RxApp.MainThreadScheduler)
                //                                     //.Sample(TimeSpan.FromMilliseconds(200d), RxApp.MainThreadScheduler)
                //                                     .InvokeCommand(ViewModel.CycleUnits)
                //                                     .DisposeWith(disposableRegistration);
           // });

        }
    
        
    }
}
