using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views
{
    //internal class CompoundableIngredientViewBase : ReactiveUserControl<CompoundableIngredientViewModel> { }
    //[AddINotifyPropertyChangedInterface]
    public partial class CompoundableIngredientView : IngredientViewBase, IViewFor<CompoundableIngredientViewModel> //Reactive UserControl worked but implementation did not have inheritence from Base view for all IngredientControls. Not sure how inheirtance of View would work in relation to binding the VM.
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
      .Register(nameof(ViewModel), typeof(CompoundableIngredientViewModel), typeof(CompoundableIngredientView), null);


        public CompoundableIngredientViewModel ViewModel
        {
            get => (CompoundableIngredientViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);

        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CompoundableIngredientViewModel)value;
        }

        public CompoundableIngredientView()
        {
            InitializeComponent();
            this.WhenActivated(disposableRegistration =>
            {

                //this.OneWayBind(ViewModel,
                //    viewModel => viewModel.IsAvailable,
                //    view => view.searchResultsListBox.Visibility)
                //    .DisposeWith(disposableRegistration);


               // this.OneWayBind(ViewModel,
               //    viewModel => viewModel.MaxHelperStepFrequency,
               //    viewModel => viewModel.ingredientSlider.StepFrequency,
               //    this.ViewModelToViewDecimalDouble
               //    /*this.ViewToViewModelConverterFunc*/)
               //.DisposeWith(disposableRegistration);

              //  this.OneWayBind(ViewModel,
              //    viewModel => viewModel.MaxHelperStepFrequency,
              //    viewModel => viewModel.stepFrequency.Text,
              //    ViewModelToViewConverterFuncDecimalString
              //    /*this.ViewToViewModelConverterFunc*/)
              //.DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IngredientItemProperty.Ingredient.Name,
                    view => view.ingredientNameText.Text)
                    .DisposeWith(disposableRegistration);

                //this.OneWayBind(ViewModel,
                //    viewModel => viewModel.SliderAmount,
                //    view => view.ingredientAmount.Text,
                //    ViewModelToViewConverterFuncDecimalString
                //    /*this.ViewModelToViewConverterFuncDoubleString*/)
                //    //this.ViewToViewModelConverterFunc)
                //    .DisposeWith(disposableRegistration);


                //The Ingredient"Slider" Amount is modified by a method and then updated to the UI
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SliderAmount,
                    view => view.ingredientSlider.Value,
                     this.ViewModelToViewDecimalDouble)//,
                    //this.ViewToViewModelDecimalDouble)
                    .DisposeWith(disposableRegistration);

                Observable
                        .FromEventPattern(ingredientSlider, "ValueChanged")
                                      .Select(x => ((Slider)x.Sender).Value)
                                      .Throttle(TimeSpan.FromMilliseconds(250d), RxApp.MainThreadScheduler)
                                      //.Sample(TimeSpan.FromMilliseconds(200d), RxApp.MainThreadScheduler)
                                      .InvokeCommand(ViewModel.SetIngredientOnSliderMove)
                                      .DisposeWith(disposableRegistration);
                /*.ObserveOn(RxApp.MainThreadScheduler)*///This is done on the creation of the 

                //sliderObservable.Subscribe(x => Debug.WriteLine(x + "From Subscription"));
                //Debug.WriteLine(sliderObservable + "THIS IS OBSERVABLEEEEEEEE");
               


                //factorAmounts.Events().DoubleTapped
                //.Select(a => a.OriginalSource)
                //.InvokeCommand(this, x => x.ViewModel.CycleUnits)
                //   .DisposeWith(disposableRegistration);


                //this.OneWayBind(ViewModel,
                //   vm => vm.ReactiveIngredientMaxAmount,
                //   view => view.ingredientAmount.Text,
                //   ViewModelToViewConverterFuncDecimalString
                //   )
                //   .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    vm => vm.DisplayUnitValues,
                    view => view.factorAmounts.ItemsSource)
                    .DisposeWith(disposableRegistration);

                //this.OneWayBind(ViewModel,
                //  vm => vm.PolicyStrings,
                //  view => view.policyDescriptions.ItemsSource)
                //  .DisposeWith(disposableRegistration);

                //this.BindCommand(
                //    this.ViewModel,
                //    viewModel => viewModel.RefreshMaxCommand,
                //    view => view.ingredientSlider,
                //    ingredientSlider.Events().DragOver);

                //ingredientSlider.Events().DragOver
                //.Select(a => a.Unit.Default)
                //.InvokeCommand(this, x => x.ViewModel.RefreshMaxCommand)
                //   .DisposeWith(disposableRegistration);


            });
        }

       

        private string ViewModelToViewConverterFuncDoubleString(double amount)
        {
            return amount.ToString();
        }

        private double ViewToViewModelConverterFuncDoubleString(string value)
        {
            double.TryParse(value, out double returnValue);
            return returnValue;
        }

        private string ViewModelToViewConverterFuncDecimalString(decimal amount)
        {

            return amount.ToString();
        }

        private decimal ViewToViewModelConverterFuncDecimalString(string value)
        {
            decimal.TryParse(value, out decimal returnValue);
            return returnValue;
        }


        private double ViewModelToViewDecimalDouble(decimal amount)
        {
            return decimal.ToDouble(amount);
        }

        private decimal ViewToViewModelDecimalDouble(double value)
        {

            return Convert.ToDecimal(value);
        }
    }
}
