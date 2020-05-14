using PropertyChanged;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Interfaces;
using UWPLockStep.ViewModels.DisplayObjects;

namespace UWPLockStep.ViewModels
{
    
    public interface IIngredientViewModel : IReactiveObject
    {
        //how will the Order be referenced from this VM? I could pass the context (as this) into the ctor. If passed as a ref, will that allow binding?
        string IngredientViewMModelId { get; }
        IngredientItem IngredientItemProperty { get; }
        ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>> PolicyEvaluatorsHelper { get; }
        UnitValueBase SetGuidance();
        decimal SliderAmount { get; set; }
        void AdjustAmount();
        ReactiveCommand<double, Unit> SetIngredientOnSliderMove { get; set; }
        IList<IngredientFactorUnitValueViewModel> DisplayUnitValues { get; set; }

    }
}

