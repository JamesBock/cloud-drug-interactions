using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// TODO: Override equate and Create tests
/// </summary>
    public abstract class UnitEnumeration : IComparable
    {
        private readonly int _value;
        private readonly string _name;
        private readonly string _abbreviation;
        private readonly decimal _baseConversion;

        protected UnitEnumeration()
        {
        }

        protected UnitEnumeration(int value, string name, string abbreviation, decimal baseConversion)
        {
            _value = value;
            _name = name;
            _abbreviation = abbreviation;
            _baseConversion = baseConversion;
        }
        //public UnitEnumeration Unit { get; protected set; }

        public int Value
        {
            get { return _value; }
        }

        public virtual string Name
        {
            get { return _name; }
        }

        public virtual string Abbreviation
        {
            get { return _abbreviation; }
        }

        public decimal BaseConversion
        {
            get { return _baseConversion; }
        }

        public override string ToString()
        {
            return Name;
        }
        //public abstract UnitEnumeration SetToBase();

        //this is close to replacing all the individual code for setting to base  in each class
        //public UnitEnumeration SetToBase<T>() where T : UnitEnumeration, new()
        //{
        //    return GetAll<T>().First(u => u.Value == 0);
        //}
      

        public virtual decimal ToBase(decimal amount) 
        {
            return amount / BaseConversion;
        }
        public virtual decimal FromBase(decimal amount)
        {
            return BaseConversion * amount;
        }

        //public virtual void ToBase(ref decimal amount)
        //{
        //    amount /= BaseConversion;
        //}
        //public virtual void FromBase(ref decimal amount)
        //{
        //    amount *= BaseConversion;
        //}
        // public abstract decimal Convert<T>(decimal amount, T from, T to); //where T : UnitEnumeration;

        public static IEnumerable<T> GetAll<T>() where T : UnitEnumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();

                if (info.GetValue(instance) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UnitEnumeration otherValue))
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : UnitEnumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((UnitEnumeration)other).Value);
        }
        public static int AbsoluteDifference(UnitEnumeration firstValue, UnitEnumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        } 
    }
}
