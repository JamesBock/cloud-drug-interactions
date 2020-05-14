using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPLockStep.Domain.Common.Units
{
    //[AddINotifyPropertyChangedInterface]
    //public class IntermediatePolyWithUnitValueValue : ReactiveObject
    //{


    //    public IntermediatePolyWithUnitValue Unit { get; }
    //    //[Reactive]
    //    public decimal Amount => _amount.Value;
    //    private readonly ObservableAsPropertyHelper<decimal> _amount;

    //    public IntermediatePolyWithUnitValueValue(IntermediatePolyWithUnitValue unit, decimal amount)
    //    {
    //       _amount = Observable.Return<decimal>(amount).ToProperty(this, x=>x.Amount);

    //        Unit = unit;
    //    }
    //}

    public class IntermediatePolyWithUnitValueValue : ReactiveObject
    {


        public IntermediatePolyWithUnitValue Unit { get; }
        [Reactive]
        public decimal Amount
        {
            get;
            set;
        }

        public IntermediatePolyWithUnitValueValue(IntermediatePolyWithUnitValue unit, decimal amount)
        {

            Amount = amount;
            Unit = unit;
        }
        
        public IntermediatePolyWithUnitValueValue GetCopy()
        {
            return MemberwiseClone() as IntermediatePolyWithUnitValueValue;
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            ((IReactiveObject)this).RaisePropertyChanged(args);
        }
    }
}
