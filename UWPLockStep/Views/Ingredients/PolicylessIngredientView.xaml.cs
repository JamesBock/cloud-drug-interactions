using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPLockStep.Views
{
    public partial class PolicylessIngredientView : IngredientViewBase,  IViewFor<PolicylessIngredientViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
     .Register(nameof(ViewModel),
            typeof(PolicylessIngredientViewModel),
            typeof(PolicylessIngredientView), null);

        public PolicylessIngredientViewModel ViewModel
        {
            get => (PolicylessIngredientViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);

        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PolicylessIngredientViewModel)value;
        }
        public PolicylessIngredientView()
        {
            InitializeComponent();
            this.WhenActivated(disposableRegistration =>
            {

                //this.OneWayBind(ViewModel,
                //    viewModel => viewModel.IsAvailable,
                //    view => view.searchResultsListBox.Visibility)
                //    .DisposeWith(disposableRegistration);

                this.OneWayBind(ViewModel,
                    viewModel => viewModel.IngredientItemProperty.Ingredient.Name,
                    view => view.ingredientNameText.Text)
                    .DisposeWith(disposableRegistration);

                //this.Bind(ViewModel,
                //    viewModel => viewModel.SearchTerm,
                //    view => view.searchTextBox.Text)
                //    .DisposeWith(disposableRegistration);
            });
        }
    }
}
