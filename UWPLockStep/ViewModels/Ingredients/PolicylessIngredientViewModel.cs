using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class PolicylessIngredientViewModel : IngredientViewModelBase, IIngredientViewModel
    {
      
        public PolicylessIngredientViewModel(IngredientItem ingredient, ObservableCollection<IPolicyEvaluationService<FactorBase>> policyEvaluatiors/*ICollection<ICombinablePolicy<FactorBase>> policies , Patient patient, OrderItemBase currentOrder*/) : base(ingredient, policyEvaluatiors)
        {

         
            // PolicyEvaluators = IngredientItemProperty.Ingredient.FactorItems.GroupBy(f => f.ItemFactor).Select(f => new PolicyEvaluationService<FactorBase>(f.Key, policies, patient, currentOrder)).ToList();


        }

        public override UnitValueBase SetGuidance()
        {
            return null;
        }
    }
}
