using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.Domain.Interfaces;
using UWPLockStep.ViewModels;

namespace UWPLockStep.Helpers
{
    public static class IngredientViewModelFactory
    {
        public static IIngredientViewModel Create(IngredientItem ingredient, ICollection<ICombinablePolicy<FactorBase>> policies/*TODO: there may be a good reason why you would not be able to use the Interface Policy vs the abstract class*/, ICollection<IPolicyEvaluationService<FactorBase>> policyEvaluatiors)
        {
            //TODO: this class should filter the PolicyEvaluators for the IngredientVM
            ObservableCollection<IPolicyEvaluationService<FactorBase>> evalCollection;
            var hasNoLimit = false;
            var hasNoRecs = false;

            var policyQuery = policies.Select(policy => policy)
                                     .Where(p => ingredient.Ingredient.FactorItems
                                     .Select(f => f.FactorId)
                                     .Any(fg => p.Target.Id == fg))
                                     .ToList();


            evalCollection = new ObservableCollection<IPolicyEvaluationService<FactorBase>>( policyEvaluatiors
                    .Select(pe => pe)
                    .Where(p => ingredient.Ingredient.FactorItems
                    .Select(f => f.FactorId)
                    .Any(fg => fg == p.Target.Id)));

            //Is the EvalService's Target being set properly?

            if (policyQuery.Count() > 0)
            {
                hasNoLimit = policyQuery.All(q => q
                .GuidanceValues[PolicyGuidance.Maximum] == null);

                hasNoRecs = policyQuery.All(q => q.GuidanceValues[PolicyGuidance.RecommendationLow] == null && q.GuidanceValues[PolicyGuidance.RecommendationHigh] == null);


                switch (hasNoLimit)
                {
                    case false:
                        return new CompoundableIngredientViewModel(ingredient, evalCollection);//where can the EvalServices come from

                    case true:
                        if (!hasNoRecs)
                        {
                            return new RecommendationIngredientViewModel(ingredient, evalCollection);
                        }
                        else return new BottomOnlyIngredientViewModel(ingredient, evalCollection);

                    default: return new PolicylessIngredientViewModel(ingredient, evalCollection);

                }

            }
            else return new PolicylessIngredientViewModel(ingredient, evalCollection);
        }
    }
}
