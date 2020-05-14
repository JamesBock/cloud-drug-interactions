using System;
using System.Collections.Generic;
using System.Text;

namespace UWPLockStep.Domain.Common
{
    public class GastroIntestinalRoute : AdministrationRouteEnumeration
    {
        public static readonly GastroIntestinalRoute Oral
            = new GastroIntestinalRoute(0, "Oral");
        public static readonly GastroIntestinalRoute NasoGastric
          = new GastroIntestinalRoute(1, "NasoGastric"); 
        public static readonly GastroIntestinalRoute Orogastric       
            = new GastroIntestinalRoute(2, "OroGastric"); 
        public static readonly GastroIntestinalRoute NasoJejunal
           = new GastroIntestinalRoute(3, "NasoJejunal");

        protected GastroIntestinalRoute() { }
        protected GastroIntestinalRoute(int value, string name) : base (value, name) { }
    }
}
