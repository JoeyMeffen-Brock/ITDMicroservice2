using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService
{
    public class Flight
    {
        public string Carrier { get; set; }
        public bool IsITDEligible { get; set; }

        public Flight(string carrier, bool isITDEligible)
        {
            Carrier = carrier;
            IsITDEligible = isITDEligible;
        }
    }
}
