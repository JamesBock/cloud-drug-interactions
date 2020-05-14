using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UWPLockStep.Domain.Common
{
    public abstract class AdministrationRouteEnumeration : IComparable
    {
        private readonly int _value;
        private readonly string _name;

        protected AdministrationRouteEnumeration()
        {
        }

        protected AdministrationRouteEnumeration(int value, string name)
        {
            _value = value;
            _name = name;
        }

        public int Value
        {
            get { return _value; }
        }

        public virtual string Name
        {
            get { return _name; }
        }

        public int CompareTo(object other)
        {
            return Value.CompareTo(((AdministrationRouteEnumeration)other).Value);
        }


        public static IEnumerable<T> GetAll<T>() where T : AdministrationRouteEnumeration, new()
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
            if (!(obj is AdministrationRouteEnumeration otherValue))
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
    }
}
