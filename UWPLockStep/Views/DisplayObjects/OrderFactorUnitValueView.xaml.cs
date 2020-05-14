using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ViewModels.DisplayObjects;
using Windows.UI.Xaml;

namespace UWPLockStep.Views.DisplayObjects
{
    public partial class OrderFactorUnitValueView : FactorUnitValueView, IViewFor<OrderFactorUnitValueViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
    .Register(nameof(ViewModel),
           typeof(OrderFactorUnitValueViewModel),
           typeof(OrderFactorUnitValueView), null);

        public OrderFactorUnitValueViewModel ViewModel
        {
            get => (OrderFactorUnitValueViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);

        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (OrderFactorUnitValueViewModel)value;
        }

        public OrderFactorUnitValueView() : base()
        {

            InitializeComponent();
            this.WhenActivated(disposableRegistration =>
            {

                //this.OneWayBind(ViewModel,
                //    viewModel => viewModel.IsAvailable,
                //    view => view.searchResultsListBox.Visibility)
                //    .DisposeWith(disposableRegistration);
                //this.OneWayBind(ViewModel,
                //       viewModel => viewModel.BigDisplayUnit,
                //       view => view.factorAmount2.Text)
                //       .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                        viewModel => viewModel.DisplayString,
                        view => view.factorAmount.Text)
                        .DisposeWith(disposableRegistration);

                factorAmount.Events().DoubleTapped
                     .Select(x => Unit.Default)
                     .InvokeCommand(ViewModel.CycleUnits)
                     .DisposeWith(disposableRegistration);


                //Observable.FromEventPattern(factorAmount, "DoubleTapped")
                //                                     .Select(x => Unit.Default)
                //                                     //.Throttle(TimeSpan.FromMilliseconds(60d), RxApp.MainThreadScheduler)
                //                                     //.Sample(TimeSpan.FromMilliseconds(200d), RxApp.MainThreadScheduler)
                //                                     .InvokeCommand(ViewModel.CycleUnits)
                //                                     .DisposeWith(disposableRegistration);
            });

        }
    }
}
