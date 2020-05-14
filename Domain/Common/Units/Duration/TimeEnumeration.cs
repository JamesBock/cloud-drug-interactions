using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UWPLockStep.Domain.Common.Units
{
    public abstract class TimeEnumeration : IComparable<TimeEnumeration>
    {
        private readonly int _value;
        private readonly string _name;
        private readonly string _abbreviation;
        private readonly long _time;

        protected TimeEnumeration()
        {
        }
        protected TimeEnumeration(int value, string name, string abbreviation, long time)
        {
            _value = value;
            _name = name;
            _abbreviation = abbreviation;
            _time = time;
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

        public long Time
        {
            get { return _time; }
        }


        public virtual decimal FromBase(decimal amount)
        {
            return amount / Time;
        }

        public virtual decimal ToBase(decimal amount)
        {
            return Time * amount;
        }

        
        public static IEnumerable<T> GetAll<T>() where T : TimeEnumeration, new()
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
            var otherValue = obj as TimeEnumeration;

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

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : TimeEnumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public virtual int CompareTo(TimeEnumeration other)//overridden in DurationValue class
        {
            return Value.CompareTo(other.Value);
        }
        public static int AbsoluteDifference(TimeEnumeration firstValue, TimeEnumeration secondValue)
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
