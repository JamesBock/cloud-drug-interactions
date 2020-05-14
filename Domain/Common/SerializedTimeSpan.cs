using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UWPLockStep.Domain.Common
{
    public class SerializedTimeSpan
    {
        private TimeSpan m_TimeSinceLastEvent;

        // Public Property - XmlIgnore as it doesn't serialize anyway
        [XmlIgnore]
        public TimeSpan TimeSinceLastEvent
        {
            get { return m_TimeSinceLastEvent; }
            set { m_TimeSinceLastEvent = value; }
        }

        // Pretend property for serialization
        [XmlElement("TimeSinceLastEvent")]
        public long TimeSinceLastEventTicks
        {
            get { return m_TimeSinceLastEvent.Ticks; }
            set { m_TimeSinceLastEvent = new TimeSpan(value); } //an overload for the TimeSpan ctor is a long, which .Ticks is 
        }

    }
}
