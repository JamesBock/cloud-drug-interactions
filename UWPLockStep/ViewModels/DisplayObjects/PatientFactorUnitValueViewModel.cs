using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.ViewModels.DisplayObjects
{
    public class PatientFactorUnitValueViewModel : FactorUnitValueViewModel
    {
        public PatientFactorUnitValueViewModel(FactorItem displayFactorItem, UnitValueBase initializerUnitValue, IPolicyEvaluationService<FactorBase> policyEvaluationService, OrderItemBase order, ICollection<IIngredientViewModel> viewModel) : base(displayFactorItem, initializerUnitValue, policyEvaluationService/*, order, viewModel*/)
        {
            CurrentOrder = order;
            ViewModel = viewModel;


            //TODO: Should Instant Policies be shown in the Patient Context?

            if (policyEvaluationService != null)
            {

            DurationPolicyList = policyEvaluationService.PolicyList.Select(p => p).Where(p => p.GetType().IsAssignableFrom(typeof(FactorWeightDurationPolicy))).ToList();
            }

                var ingredientControlCollection = new SourceCache<IIngredientViewModel, string>(vm => vm.IngredientViewMModelId);

            ingredientControlCollection.AddOrUpdate(ViewModel);

            ingredientControlCollection.Connect()
                                       .AutoRefresh(d => d.SliderAmount)
                                       .ObserveOn(RxApp.MainThreadScheduler)
                                        //.Bind(out _ingredientControls)
                                         .Subscribe(x =>
                                         {
                                             if (policyEvaluationService != null && DurationPolicyList.Count > 0)
                                             {
                                                 PassThroughString = $"{DisplayFactorItem.ItemFactor.Name}: {DurationPolicyList[DisplayCount % DurationPolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume))) + initializerUnitValue)}";


                                             }
                                             else
                                             {
                                                 PassThroughString = $"{DisplayFactorItem.ItemFactor.Name}: {order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume))) + initializerUnitValue}";

                                             }
                                         });






            this.WhenAnyValue(x => x.CurrentOrder.TotalVolume, y => y.DisplayCount, z => z.PassThroughString)
                  .Select(x =>
                  {
                      if (policyEvaluationService != null && DurationPolicyList.Count > 0)
                      {

                          return $"{DisplayFactorItem.ItemFactor.Name}: {DurationPolicyList[DisplayCount % DurationPolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume))) + initializerUnitValue)}";

                      }
                      else
                      {
                          return PassThroughString;

                      }



                  }
                  )
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .ToProperty(this, nameof(DisplayString), out _displayString);

        }
        public IList<ICombinablePolicy<FactorBase>> DurationPolicyList { get; set; }
        [Reactive]
        public OrderItemBase CurrentOrder { get; set; }
        public ICollection<IIngredientViewModel> ViewModel { get; }
        [Reactive]
        public string PassThroughString { get; set; }
    }
}
