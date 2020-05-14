using ReactiveUI;
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
    public interface IFactorUnitValueViewModel : IReactiveObject //Could further abstract this to IUnitValueViewModel<T>
    {
        FactorItem DisplayFactorItem { get; }

        UnitValueBase InitializerUnitValue { get; set; } //TODO: Use this as ToolTip

        UnitValueBase DisplayUnitValue { get; }

        string DisplayString {get;}

        int DisplayCount { get; set; }

        ReactiveCommand<Unit, Unit> CycleUnits { get; set; }

        IPolicyEvaluationService<FactorBase> PolicyEvaluationService { get; set; }

    }

    
}
