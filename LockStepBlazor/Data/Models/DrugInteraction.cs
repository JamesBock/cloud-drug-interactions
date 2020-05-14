using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepBlazor.Data.Models
{
    public class DrugInteraction
    {
        
        public string Severity { get; set; }
       
        public string Description { get; set; }

        public List<(string, Uri)> TupList { get; set; }
    }
}
