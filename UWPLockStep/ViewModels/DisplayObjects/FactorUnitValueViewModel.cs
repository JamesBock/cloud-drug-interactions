using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.ViewModels.DisplayObjects
{
    public abstract class FactorUnitValueViewModel : ReactiveObject, IFactorUnitValueViewModel
    {
        public FactorUnitValueViewModel(FactorItem displayFactorItem, UnitValueBase initializerUnitValue, IPolicyEvaluationService<FactorBase> policyEvaluationService = null)
        {
            DisplayFactorItem = displayFactorItem;
            InitializerUnitValue = initializerUnitValue;
            PolicyEvaluationService = policyEvaluationService;//Could create a basic Policy to be used as a formating template


            this.WhenAnyValue(x => x.InitializerUnitValue)
               .ToProperty(this, nameof(DisplayUnitValue), out _displayUnit)
               .Dispose();

            

            CycleUnits = ReactiveCommand.Create(() =>
            {
                DisplayCount++;
              
            }/*, canExecuteIngredientChange*/, outputScheduler: RxApp.MainThreadScheduler);
        }

        
        public FactorItem DisplayFactorItem { get; }

        [Reactive]
        public UnitValueBase InitializerUnitValue { get; set; } //TODO: Use this as ToolTip

        protected ObservableAsPropertyHelper<UnitValueBase> _displayUnit;
        public UnitValueBase DisplayUnitValue => _displayUnit.Value;

        protected ObservableAsPropertyHelper<string> _displayString;
        public string DisplayString => _displayString.Value;

        [Reactive]
        public int DisplayCount { get; set; }

        public ReactiveCommand<Unit, Unit> CycleUnits { get; set; }

        public IPolicyEvaluationService<FactorBase> PolicyEvaluationService { get; set; }

        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            ((IReactiveObject)this).RaisePropertyChanged(args);
        }
    }
}
