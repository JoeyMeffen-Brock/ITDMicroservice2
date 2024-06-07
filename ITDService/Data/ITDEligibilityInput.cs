using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.SmartSuite.Events;

namespace BrockSolutions.ITDService.Data
{
    public record ITDEligibilityInput
    {
        public List<FlightLeg> FlightLegs { get; set; } = new List<FlightLeg>();
        public string StationCode { get; set; } = "";
        public bool HasCheckedBag { get; set; } = false;
        public string ITDEligibilityString { get; set; } = "";
        //public string FlightMarket { get; set; } = "";
    }
}
