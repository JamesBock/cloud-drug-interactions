using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;
using UWPLockStep.ViewModels.Orders;

namespace UWPLockStep.ViewModels.DisplayObjects
{
    public class OrderFactorUnitValueViewModel : FactorUnitValueViewModel
    {
        public OrderFactorUnitValueViewModel(FactorItem displayFactorItem, UnitValueBase initializerUnitValue, IPolicyEvaluationService<FactorBase> policyEvaluationService, OrderItemBase order, ICollection<IIngredientViewModel> viewModel) : base(displayFactorItem, initializerUnitValue, policyEvaluationService)
        {

            CurrentOrder = order;
            ViewModel = viewModel;



            var ingredientControlCollection = new SourceCache<IIngredientViewModel, string>(vm => vm.IngredientViewMModelId);

            ingredientControlCollection.AddOrUpdate(ViewModel);

            ingredientControlCollection.Connect()
                                      //.DeferUntilLoaded()/*.Throttle(TimeSpan.FromMilliseconds(1000))*/
                                      .AutoRefresh(d => d.SliderAmount)//.WhereReasonsAreNot(ChangeReason.Refresh)
                                      //.IgnoreSameReferenceUpdate()//TODO: DOES THIS DO ANYTHING?!
                                       .ObserveOn(RxApp.MainThreadScheduler)
                                         //.Bind(out _ingredientControls)
                                         .Subscribe(x =>
                                         {
                                             if (PolicyEvaluationService == null)
                                             {

                                                 PassThroughString = $"{DisplayFactorItem.ItemFactor.Name}: {order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume)))}";

                                             }
                                             else
                                             {

                                                 PassThroughString = $"{DisplayFactorItem.ItemFactor.Name}: {PolicyEvaluationService.PolicyList[DisplayCount % PolicyEvaluationService.PolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume))))}";
                                             }
                                         });






            this.WhenAnyValue(x => x.CurrentOrder.TotalVolume, y => y.DisplayCount, z => z.PassThroughString)
                  .Select(x =>
                  {
                      if (PolicyEvaluationService == null)
                      {

                          return $"{DisplayFactorItem.ItemFactor.Name}: {order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume)))}";

                      }
                      else
                      {

                          return $"{DisplayFactorItem.ItemFactor.Name}: {PolicyEvaluationService.PolicyList[DisplayCount % PolicyEvaluationService.PolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, order.IngredientItems.SelectMany(f => f.Ingredient.FactorItems).Where(f => f.FactorId == DisplayFactorItem.FactorId).Aggregate(UnitValueFactory.Create(DisplayFactorItem.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(order.IngredientItems.Where(i => i.Ingredient.FactorItems.Any(fid => fid.FactorId == DisplayFactorItem.FactorId)).Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(order.TotalVolume))))}";
                      }



                  }
                  )
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .ToProperty(this, nameof(DisplayString), out _displayString);

        }

        [Reactive]
        public OrderItemBase CurrentOrder { get; set; }
        public ICollection<IIngredientViewModel> ViewModel { get; }
        [Reactive]
        public string PassThroughString { get; set; }
    }
}
