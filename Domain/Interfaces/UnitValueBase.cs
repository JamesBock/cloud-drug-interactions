using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Exceptions;
using UWPLockStep.Domain.Common.Units;

namespace UWPLockStep.Domain.Interfaces
{
    public abstract class UnitValueBase :  IComparable<UnitValueBase>
    {
        protected decimal _amount;

        public IUnit Unit { get;  protected set; }

        public decimal Amount { get => _amount;  protected set =>  _amount = value; }

        public abstract void Convert(UnitEnumeration newUnit);

        public int CompareTo(UnitValueBase other)
        {
            if (ReferenceEquals(other, null)) return 1;
            try
            {
                other.Convert(Unit.UnitEnumerator);
                return Amount.CompareTo(other.Amount);
            }
            catch (UnitConversionException)
            {

                throw new UnitConversionException(other.Unit.UnitEnumerator, Unit.UnitEnumerator);
            }
        }

        public static bool operator >(UnitValueBase left, UnitValueBase right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool operator <(UnitValueBase left, UnitValueBase right)
        {
            return left.CompareTo(right) == -1;
        }

        public static UnitValueBase operator +(UnitValueBase a, UnitValueBase b)
        {
            if (a.Unit.UnitEnumerator == b.Unit.UnitEnumerator)
            {
                return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount + b.Amount);
            }
            b.Convert(a.Unit.UnitEnumerator);
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount + b.Amount);
        }

        public static UnitValueBase operator -(UnitValueBase a, UnitValueBase b)
        {
            if (a.Unit.UnitEnumerator == b.Unit.UnitEnumerator)
            {
                return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount - b.Amount);
            }
            b.Convert(a.Unit.UnitEnumerator);
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount - b.Amount);
        }

        public static UnitValueBase operator *(UnitValueBase a, UnitValueBase b)
        {
            if (a.Unit.UnitEnumerator == b.Unit.UnitEnumerator)
            {
                return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount * b.Amount);
            }
            b.Convert(a.Unit.UnitEnumerator);
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount * b.Amount);
        }

        public static UnitValueBase operator /(UnitValueBase a, UnitValueBase b)
        {
            if (a.Unit.UnitEnumerator == b.Unit.UnitEnumerator)
            {
                return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount / b.Amount);
            }
            b.Convert(a.Unit.UnitEnumerator);
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount / b.Amount);
        }

        public static UnitValueBase operator *(UnitValueBase a, decimal b)
        {
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount * b);
        }

        public static UnitValueBase operator *(decimal a, UnitValueBase b)
        {
            return UnitValueFactory.Create(b.Unit.UnitEnumerator, b.Amount * a);
        }

        public static UnitValueBase operator /(UnitValueBase a, long b)
        {
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount / b);
        }

        public static UnitValueBase operator /(UnitValueBase a, decimal b)
        {
            return UnitValueFactory.Create(a.Unit.UnitEnumerator, a.Amount / b);
        }

        public static UnitValueBase operator /(decimal a, UnitValueBase b)
        {
            return UnitValueFactory.Create(b.Unit.UnitEnumerator, a / b.Amount);
        }

        public override string ToString()
        {   //TODO: If you want the decimal places to be set, pass it into the Constructor and create a prop
            return $"{Math.Round(Amount, 2).ToString()}{Unit.UnitEnumerator.Abbreviation}";
        }
        //protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        //{
        //    ((IReactiveObject)this).RaisePropertyChanged(args);
        //}
    }
}
