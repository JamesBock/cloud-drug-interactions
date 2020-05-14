using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.ViewModels.DisplayObjects
{
    public class IngredientFactorUnitValueViewModel : FactorUnitValueViewModel
    {
        public IngredientFactorUnitValueViewModel(FactorItem displayFactorItem, UnitValueBase initializerUnitValue, IPolicyEvaluationService<FactorBase> policyEvaluationService, IngredientItem ingredient) : base(displayFactorItem, initializerUnitValue, policyEvaluationService)
        {
           //TODO: for some reason the amount of the bases is not updated on the first adjustment. should this also implement DynamicData to know when an Ingredient that uses another Factor as Basis changes? 

            //DisplayFactorItem = displayFactorItem;
            //InitializerUnitValue = displayUnitValue;
            //PolicyEvaluationService = policyEvaluationService;
            IngredientProperty = ingredient;
                                                        //This will break once its used outside of the CompoundedIngredients
            this.WhenAnyValue(x => x.IngredientProperty.EditableUnitValue.Amount, y => y.DisplayCount)
                  .Select(x =>

                     $"{DisplayFactorItem.ItemFactor.Name}: {PolicyEvaluationService.PolicyList[DisplayCount % PolicyEvaluationService.PolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, DisplayFactorItem.GetTotal(UnitValueFactory.Create(IngredientProperty.EditableUnitValue.Unit.UnitPairs[IngredientProperty.Ingredient.Id].Unit, IngredientProperty.EditableUnitValue.Amount)))}" ??

                     $"{DisplayFactorItem.ItemFactor.Name}: {DisplayFactorItem.GetTotal(UnitValueFactory.Create(IngredientProperty.EditableUnitValue.Unit.UnitPairs[IngredientProperty.Ingredient.Id].Unit, IngredientProperty.EditableUnitValue.Amount))}" ??

                     $"{DisplayFactorItem.ItemFactor.Name}: {InitializerUnitValue.ToString()}"

                  )
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .ToProperty(this, nameof(DisplayString), out _displayString);

        }


        [Reactive]
        public IngredientItem IngredientProperty { get; set; } 
           
        

        

        //public override string ToString()
        //{   var stingy = /*$"{DisplayFactorItem.ItemFactor.Name}: {PolicyEvaluationService.PolicyList[DisplayCount % PolicyEvaluationService.PolicyList.Count].GetPolicyDisplayUnit(PolicyEvaluationService, DisplayFactorItem.GetTotal(UnitValueFactory.Create(IngredientProperty.EditableUnitValue.Unit.UnitPairs[IngredientProperty.Ingredient.Id].Unit, IngredientProperty.EditableUnitValue.Amount)))}" ?? $"{DisplayFactorItem.ItemFactor.Name}: {DisplayFactorItem.GetTotal(UnitValueFactory.Create(IngredientProperty.EditableUnitValue.Unit.UnitPairs[IngredientProperty.Ingredient.Id].Unit, IngredientProperty.EditableUnitValue.Amount))}" ??*/
        //        $"{DisplayFactorItem.ItemFactor.Name}: {DisplayUnitValue.ToString()}";
        //    return stingy;
        //    //return $"{DisplayFactorItem.ItemFactor.Name}: {DisplayUnitValue.ToString()}";//TODO: This is good but needs to have options to display in terms of the policy.
        //    // $"{DisplayUnitValue.ToString()}"
        //}

        //public void RefreshValues()
        //{
        //    DisplayString = ToString();
        //}

     
    }
}
