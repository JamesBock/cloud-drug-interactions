using System.Reactive.Disposables;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ViewModels;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System.Reactive.Linq;
using UWPLockStep.ViewModels.Orders;

namespace UWPLockStep.Views
{
    // MainWindow class derives off ReactiveWindow which implements the IViewFor<TViewModel>
    // interface using a WPF DependencyProperty. We need this to use WhenActivated extension
    // method that helps us handling View and ViewModel activation and deactivation.
    public sealed partial class CompoundableFluidOrderView : Page, IViewFor<CompoundableFluidOrderViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
       .Register(nameof(ViewModel), typeof(CompoundableFluidOrderViewModel), typeof(CompoundableFluidOrderView), null);


        public CompoundableFluidOrderViewModel ViewModel
        {
            get => (CompoundableFluidOrderViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        public CompoundableFluidOrderView()
        {
            InitializeComponent();


            this.WhenActivated(disposableRegistration =>
            {
                //this.OneWayBind(ViewModel,
                //       viewModel => viewModel.IsAvailable,
                //       view => view.searchResultsListBox.Visibility)
                //       .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.ActivePatient.LastName,
                    view => view.patientNameTextBlock.Text)
                    .DisposeWith(disposableRegistration);

                this.Bind(ViewModel,
                    viewModel => viewModel.IngredientControls,
                    view => view.IngredientControlsListBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                //this.WhenAnyValue(view => (IIngredientViewModel)view.searchResultsListBox.SelectedItem)
                //    .BindTo(this, view => view.ViewModel.SelectedControl)
                //   .DisposeWith(disposableRegistration);

                //this.Bind(ViewModel, view => view.SelectedControl,
                //   view => view.searchResultsListBox.SelectedValuePath)
                //   .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.DisplayFactorOrderList,
                    view => view.orderFactorsListBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.DisplayFactorTimeAdjustedPatientList,
                    view => view.patientFactorsListBox.ItemsSource)
                    .DisposeWith(disposableRegistration);

                //this.OneWayBind(ViewModel,
                //    vm => vm.SelectedControl.IngredientViewMModelId,
                //    v => v.selectedObject.Text)
                //.DisposeWith(disposableRegistration);

                //ViewModel.IngredientControls.WhenAnyValue(x => x.Select(sa=>sa.SliderAmount)).

                // ViewModel.IngredientControls.WhenAnyValue(x => x.Select(sa=>sa.SliderAmount))

                //this.WhenAnyValue(v => v.ViewModel.IngredientControls.Select(sa => sa.SliderAmount))
                //    .Subscribe(ii =>
                //   {
                //       //ViewModel.IngredientControls
                //       //.ToList(x=> x).ForEach(f=> f.AdjustAmount())

                //   });


            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CompoundableFluidOrderViewModel)value;
        }
    }
}
