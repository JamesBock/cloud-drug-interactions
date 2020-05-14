using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Interfaces;

namespace UWPLockStep.Domain.Common
{
    public class Elements
    {
        public IEnumerable<Elements> ElementList { get; set; }
        public string Name { get; set; }
        public decimal AtomicMass { get; set; }
        public int Valence { get; set; }

        public Elements(ref IUnit poly,XElement xmlElement)
        {
            Name = (string)xmlElement.Element("Name");
            AtomicMass = (decimal)xmlElement.Element("AtomicMass");
            Valence = (int)xmlElement.Element("Valence");
            //poly = UnitFactory.GetUnit(Array.Empty<KeyValuePair<UnitEnumeration, decimal>>(); { (Equivalents.Milliequivalent, Valence); (Mass.Gram, AtomicMass),(AmountOfElement.Millimole, 1m)});

            poly = UnitFactory.Create(new MonoUnitValue(new MonoUnit(Equivalents.Milliequivalent), Valence), new MonoUnitValue(new MonoUnit(Mass.Gram), AtomicMass), new MonoUnitValue(new MonoUnit(AmountOfElement.Millimole), 1m));
        }


        public void GetElements(IUnit unit)
        {
            var document = XDocument.Load(@"C:\Users\James\source\repos\UWPLockStepSolution\Domain\Common\Elements.xml");
            ElementList =
                 document.Descendants("ElementData")
                 .Select(element => new Elements(ref unit, element.Element("")));

            //ElementObject = new Elements(ref unit, query);
        }
    }
}
