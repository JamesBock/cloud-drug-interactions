using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public class AmountEnumeration : IComparable<AmountEnumeration>
    {
        private readonly int _value;
        private readonly string _name;
        private readonly string _abbreviation;
        //private readonly decimal _atomicWeight;

        protected AmountEnumeration()
        {
        }
        protected AmountEnumeration(int value, string name, string abbreviation/*, decimal atomicWeight*/)
        {
            _value = value;
            _name = name;
            _abbreviation = abbreviation;
           //_atomicWeight = atomicWeight;
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

        //public decimal AtomicWeight
        //{
        //    get { return _atomicWeight; }
        //}


        //public virtual decimal FromBase(decimal amount)
        //{
        //    return amount / AtomicWeight;
        //}

        //public virtual decimal ToBase(decimal amount)
        //{
        //    return AtomicWeight * amount;
        //}

        /// <summary>
        /// Needs a public parameterless ctor...This does nothing for me...Maybe this is only meant to be used in the other methods of this class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAll<T>() where T : AmountEnumeration, new()
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



        public override bool Equals(object obj) //This equates the Duration only not the DurationValue
        {
            var otherValue = obj as AmountEnumeration;

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

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : AmountEnumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public virtual int CompareTo(AmountEnumeration other)//overridden in DurationValue class
        {
            return Value.CompareTo(other.Value);
        }
        public static int AbsoluteDifference(AmountEnumeration firstValue, AmountEnumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
