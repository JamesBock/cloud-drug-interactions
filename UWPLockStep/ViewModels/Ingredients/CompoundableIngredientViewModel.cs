using DynamicData.Binding;
using DynamicData;
using PropertyChanged;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;
using System.Reactive;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Diagnostics;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Policies;
using UWPLockStep.ViewModels.DisplayObjects;

namespace UWPLockStep.ViewModels
{
    //[AddINotifyPropertyChangedInterface]
    public class CompoundableIngredientViewModel : IngredientViewModelBase, IIngredientViewModel
    {
        //public ICollection<FactorItem> FactorsWithPolicies { get; }

        public IList<ICombinablePolicy<FactorBase>> Policies { get; set; }

        //private readonly ObservableAsPropertyHelper<decimal> _maxHelper;
        //public decimal MaxHelper => _maxHelper.Value;

        //[Reactive]
        //public decimal SecondMaxHelper { get; set; }

        [Reactive]
        public decimal MaxHelperStepFrequency { get; set; }

       
        public CompoundableIngredientViewModel(IngredientItem ingredient, ObservableCollection<IPolicyEvaluationService<FactorBase>> policyEvaluatiors
            ) : base(ingredient, policyEvaluatiors)
        {

            //FactorsWithPolicies = ingredient.Ingredient.FactorItems
            //    .Join(PolicyEvaluatorsHelper,
            //    fi => fi.ItemFactor.Id,
            //    pe => pe.Target.Id,
            //    (fi, pe) => fi)
            //   .ToList();


            //Each Ingredient is ordered "Primarily" by one Factor, this selects the first Factor in the Ingredient list and uses it to base the slider ticks off of.

            var primary = IngredientItemProperty.Ingredient.FactorItems
                                .Join(PolicyEvaluatorsHelper,
                                fi => fi.FactorId,
                                pe => pe.Target.Id,
                                (fi, pe) =>

                                new
                                {
                                    PrimaryFactor = fi,
                                    Eval = pe
                                })
                                .FirstOrDefault();//This ViewModel shouldn't be used if there are no Policies


            
            PolicyEvaluatorsHelper.OrderByDescending(pe => pe.RulingMaximum);

            foreach (var item in PolicyEvaluatorsHelper)
            {
                item.PolicyList.Where(p => primary.PrimaryFactor.FactorId == p.Target.Id);
            }

            Policies = PolicyEvaluatorsHelper
                        .SelectMany(x => x.PolicyList.Select(p => p))
                        .ToList();
            try
            {

                MaxHelperStepFrequency = 1m / (primary.PrimaryFactor.FactorPerIngredientUnit.UnitPairs.Values.First().Amount * primary.PrimaryFactor.GetIngredientAmount(primary.Eval.RulingMaximum).Amount) * .0000001m;
            }
            catch (DivideByZeroException)
            {

                MaxHelperStepFrequency = 0m; //.00000000001; This might fix it
            }

            SetDisplayUnitValue();


            SetIngredientOnSliderMove = ReactiveCommand.Create<double>(x =>

            {
                IngredientItemProperty.SetIngredientAmount(
                   (SetGuidance() + UnitValueFactory.Create(IngredientItemProperty.EditableUnitValue.Unit.UnitPairs[IngredientItemProperty.Ingredient.Id].Unit, IngredientItemProperty.EditableUnitValue.Amount)).Amount * (decimal)x);

                //trying to order PolicyEval by IngredientAmount of Ruling max
                //var pp = PolicyEvaluatorsHelper.Select(pe => new
                //{
                //   Name = pe.Target.Name,
                //   IngredientAmount= IngredientItemProperty.Ingredient.FactorItems
                //                               .Select(fi => fi.GetIngredientAmount(pe.RulingMaximum))
                //});

                //Debug.WriteLine(IngredientItemProperty.EditableUnitValue.Amount + "IngredientAmount via Command");
            //Using the sliderAmount as a percentage, this finds the max value and adds back what in the current Ingredient then assisgns that percentage to the Amount


            //DisplayUnitValues = IngredientItemProperty.Ingredient.FactorItems
            //    .Select(fi => new FactorItemUnitValue(fi, fi.GetTotal(UnitValueFactory.Create   (IngredientItemProperty.EditableUnitValue.Unit.UnitPairs                [IngredientItemProperty.Ingredient.Id].Unit, IngredientItemProperty.EditableUnitValue.Amount)))).ToList();

                //Seriously...
                foreach (var policyEval in PolicyEvaluatorsHelper)
                {
                    policyEval.GetRulingMax();
                    //Debug.WriteLine(policyEval.RulingMaximum + "RulingMax via Command");
                }
                

                try
                {

                    MaxHelperStepFrequency = 1m / (primary.PrimaryFactor.FactorPerIngredientUnit.UnitPairs.Values.First().Amount * (primary.PrimaryFactor.GetIngredientAmount(primary.Eval.RulingMaximum).Amount + IngredientItemProperty.EditableUnitValue.Amount)) * .00001m;
                }
                catch (DivideByZeroException)
                {

                    MaxHelperStepFrequency = 0m;
                }

                AdjustAmount();

            }/*, canExecuteIngredientChange*/, outputScheduler: RxApp.MainThreadScheduler);


            //this.WhenAnyValue(x => x.PolicyEvaluatorsHelper)
            //    .Select(x => (SetGuidance() + UnitValueFactory.Create(IngredientItemProperty.EditableUnitValue.Unit.UnitPairs[IngredientItemProperty.Ingredient.Id].Unit, IngredientItemProperty.EditableUnitValue.Amount)).Amount)
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .ToProperty(this, x => x.ReactiveIngredientMaxAmount, out _reactiveIngredientMaxAmount);




            //this.WhenAnyValue(x => x.SliderAmount)//Create a new Property and Run this amount through IngredientItem.Ingredient.FactorItem.Select(x=> GetTotal(x"this needs to be a UnitValueFactor") for each factorItem. This may be a propblem when trying to comunicate with other VMs    
            //                                      //.Throttle(TimeSpan.FromMilliseconds(200d))
            //  .ObserveOn(RxApp.MainThreadScheduler)
            //  .ToProperty(this, x => x.ReactiveIngredientAmount, out _reactiveIngredientAmount);


        }



        public void SetRecommendation()
        {
            //TODO: Implement button that sets the SliderAmount to the average of the Recommndations, the
        }

        public override UnitValueBase SetGuidance() //this is returning the maximum amount of an Ingredient based on all Factors in the Ingredient.
        {
           

            return IngredientItemProperty.Ingredient.FactorItems
               .Where(fi => PolicyEvaluatorsHelper.Any(pe => pe.Target.Id == fi.FactorId))
               .Select(fi => fi.GetIngredientAmount(PolicyEvaluatorsHelper
               .Where(pe => pe.Target.Id == fi.FactorId)
               .Single().RulingMaximum))
               .Min();//Will this return the lowest value always?
        }

        //public decimal GetSliderSteps()
        //{

        //        try
        //    {
        //        return 1m / (primary.PrimaryFactor.FactorPerIngredientUnit.UnitPairs.Values.First().Amount * primary.PrimaryFactor.GetIngredientAmount(primary.Eval.RulingMaximum).Amount) * .1m;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public override void AdjustAmount()
        {
            try
            {
                #region Debug vars
                //var guide = Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven);
                //var ingAmount = Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 4, MidpointRounding.ToEven);
                //var slideVar = Math.Round(Convert.ToDecimal(
                //    SliderAmount), 4);

                //var otherHalf = Math.Round(

                // Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven)

                ///
                //(Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven) + Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven))    , 4, MidpointRounding.ToEven);

                //var fulltest = Math.Round(1m - (Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven)
                //                       /
                //      (Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven) - Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 4, MidpointRounding.ToEven))), 4, MidpointRounding.ToEven);
                #endregion

                if (Math.Round(Convert.ToDecimal(
                                   SliderAmount), 4, MidpointRounding.ToEven) !=
                    Math.Round(

                 Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven)
                                 /
                (Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven) + Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven)), 4, MidpointRounding.ToEven))
                {
                    SliderAmount = Math.Round(

                 Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven)
                                   /
                (Math.Round(SetGuidance().Amount, 4, MidpointRounding.ToEven) + Math.Round(IngredientItemProperty.EditableUnitValue.Amount, 8, MidpointRounding.ToEven)), 4, MidpointRounding.ToEven);
                                     
                }
              
                Debug.WriteLine(IngredientItemProperty.EditableUnitValue.Amount + "IngredientAmount");
                //Using the sliderAmount as a percentage, this finds the max value and adds back what in the current Ingredient then assisgns that percentage to the Amount

                //foreach (var item in DisplayUnitValues)
                //{
                //    item.RefreshValues();
                //}
                //SetDisplayUnitValue();

                //Seriously...
                foreach (var policyEval in PolicyEvaluatorsHelper)
                {
                    policyEval.GetRulingMax();
                    Debug.WriteLine(policyEval.RulingMaximum + "RulingMax");
                }

              PolicyEvaluatorsHelper.OrderByDescending(pe => pe.RulingMaximum);
            }
            catch (DivideByZeroException)
            {

                SliderAmount = 0;// .99999999999m;// this might fix it?
            }
        }

        public void SetDisplayUnitValue()
            {
                DisplayUnitValues = IngredientItemProperty.Ingredient.FactorItems
              .Select(fi => new IngredientFactorUnitValueViewModel(fi, fi.GetTotal(UnitValueFactory.Create(IngredientItemProperty.EditableUnitValue.Unit.UnitPairs[IngredientItemProperty.Ingredient.Id].Unit, IngredientItemProperty.EditableUnitValue.Amount)), PolicyEvaluatorsHelper.Where(p => p.Target == fi.ItemFactor).FirstOrDefault(), IngredientItemProperty)).ToList();
        }
        //RulingMax needs to be equated to the amount of the Ingredient (through GetTotal() of the Factor. Each Ruling Property is compared to across EvalServices and the max of the slider is the Min()
        //Need a ReactiveCommand that will trigger the Rulling Property evaluations after each Ingredient is modified by the User

        //protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        //{
        //    ((IReactiveObject)this).RaisePropertyChanged(args);
        //}
    }
}
