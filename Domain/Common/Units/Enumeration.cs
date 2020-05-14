using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{/// <summary>
/// 
/// </summary>
    public abstract class Enumeration : IComparable
    {
        private readonly int _value;
        private readonly string _name;
        private readonly string _abbreviation;
        private readonly BaseUnitValue<Mass> _atomicWeight;
        private readonly BaseUnitValue<AmountOfElement> _amount;
        private readonly int _valence;

        protected Enumeration()
        {
        }

        protected Enumeration(int value, string name, string abbreviation, BaseUnitValue<Mass> atomicWeight, int valence, BaseUnitValue<AmountOfElement> amount)
        {
            _value = value;
            _name = name;
            _abbreviation = abbreviation;
            _atomicWeight = atomicWeight;
            _valence = valence;
            _amount = amount;
        }

        public int Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Abbreviation
        {
            get { return _abbreviation; }
        }

        public BaseUnitValue<Mass> AtomicWeight
        {
            get { return _atomicWeight; }
        }

        public BaseUnitValue<AmountOfElement> Amount
        {
            get { return _amount; }
        }
        public int Valence
        {
            get { return _valence; }
        }

        public override string ToString()
        {
            return Name;
        }
        //public abstract decimal ToBase(object amount);
        //{
        //return amount / BaseConversion;
        //}
       // public abstract decimal FromBase(decimal amount);
        //{
        //    return BaseConversion * amount;
        //}
        // public abstract decimal Convert<T>(decimal amount, T from, T to); //where T : Enumeration;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
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

        private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
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
            return Value.CompareTo(((Enumeration)other).Value);
        }
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }
    }
}
