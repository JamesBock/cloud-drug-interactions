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
    public class RecommendationIngredientViewModel : IngredientViewModelBase, IIngredientViewModel
    {
        private IngredientItem ingredient;
        private ObservableCollection<IPolicyEvaluationService<FactorBase>> evalCollection;

        public RecommendationIngredientViewModel(IngredientItem ingredient, ObservableCollection<IPolicyEvaluationService<FactorBase>> policyEvaluatiors) : base(ingredient, policyEvaluatiors)
        {

          

        }

        public override UnitValueBase SetGuidance()
        {
            return null;
        }
    }
}

