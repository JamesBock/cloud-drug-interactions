using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;
using UWPLockStep.ViewModels.DisplayObjects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPLockStep.ViewModels
{   //[AddINotifyPropertyChangedInterface]
    public abstract class IngredientViewModelBase : ReactiveObject, IIngredientViewModel
    {       
        [Reactive]
        public IList<IngredientFactorUnitValueViewModel> DisplayUnitValues { get; set; }

        public ReactiveCommand<double, Unit> SetIngredientOnSliderMove { get; set; }

        public IngredientItem IngredientItemProperty { get; }

        public decimal SliderAmount
        {
            get => sliderAmount;
            set => this.RaiseAndSetIfChanged(ref sliderAmount, value);
        }
        private decimal sliderAmount;

        //[Reactive]
        //public ReadOnlyObservableCollection<FactorPolicyEvaluationService> PolicyEvaluatorsHelper { get; set;  }
        protected ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>> _policyEvaluators;

        public ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>> PolicyEvaluatorsHelper => _policyEvaluators;

        public string IngredientViewMModelId { get; }

        public IngredientViewModelBase(IngredientItem ingredient, ObservableCollection<IPolicyEvaluationService<FactorBase>> policyEvaluatiors)
        {
            DisplayUnitValues = new List<IngredientFactorUnitValueViewModel>();
            IngredientViewMModelId = Guid.NewGuid().ToString().Substring(0, 5);
            IngredientItemProperty = ingredient;//.GetCopy(); // this seperateds the IngredientItemAmount in the CurrentOrder from the Other Orders, but the PolicyEvaluator does not keep the reference so its IngredientItems never increases

            //IngredientItemProperty.EditableUnitValue = IngredientItemProperty.EditableUnitValue.GetCopy();//This is actually waht is achieving the seperation...ACTUALLY, they need to be used together? If this is done for every Ingredeitn in the CurrentOrder, then the CurrentOrder is updated, it should be like cloning the Order...If Eval is passed the CurrentOrder later it could work


            _policyEvaluators = new ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>>(policyEvaluatiors);

            //policyEvaluators.AddOrUpdate(policyEvaluatiors);
            //policyEvaluators.Connect()
            //    //.AutoRefresh(p=>p.RulingMaximum)//This isn't working because these are only reference to the actual objects, which are on the OrderVM
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Bind(out _policyEvaluators)
            //    .Subscribe(
            //    x =>
            //    {
            //        //PolicyEvaluatorsHelper.OrderByDescending(pe => pe.RulingMaximum);
            //        //Debug.WriteLine(x.TotalChanges);
            //    }
            //    );


            //Seriously...
            //foreach (var policyEval in PolicyEvaluatorsHelper)
            //{
            //    policyEval.GetRulingMax();
            //    Debug.WriteLine(policyEval.RulingMaximum + "RulingMax");
            //}
            //var canExecuteIngredientChange = this.WhenAnyValue(
            //                  x => x.UnitValueBases,
            //                  x => x.Any()
            //                  );
            //This doesn't add anything as is



        }

        public abstract UnitValueBase SetGuidance();
        public virtual void AdjustAmount() { }
        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            ((IReactiveObject)this).RaisePropertyChanged(args);
        }

    }
}
