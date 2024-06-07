using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService.Data
{
    public record Flight
    {
        public enum FlightMarket
        {
            None,
            Domestic,
            International,
            Transborder
        }

        public long DepartureDateLocal { get; set; } = 0;
        public string FlightNumber { get; set; } = "";
        public string CarrierCode { get; set; } = "";
        public string DepartureStation { get; set; } = "";
        public string ArrivalStation { get; set; } = "";
        public FlightMarket Market { get; set; } = FlightMarket.None;
    }
}
