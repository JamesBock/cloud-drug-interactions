using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common
{
    public class InjectionRoute : AdministrationRouteEnumeration
    {
        public static readonly InjectionRoute IVCentral
            = new InjectionRoute(0, "Intravenous (Central)");
        public static readonly InjectionRoute IVPeripheral
          = new InjectionRoute(1, "Intravenous (Peripheral)");
        public static readonly InjectionRoute Oral
            = new InjectionRoute(2, "Oral");
        public static readonly InjectionRoute Intramuscular
            = new InjectionRoute(3, "Intramuscular");


        protected InjectionRoute() { }

        protected InjectionRoute(int value, string name) : base(value, name) { }
    }
}
