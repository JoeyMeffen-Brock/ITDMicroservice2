using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.SmartSuite.Events;

namespace BrockSolutions.ITDService.Data
{
    public class Passenger
    {
        public long BookingID { get; set; } = -1;
        public List<Bag> Bags { get; set; } = new List<Bag>();
        public List<Flight> FlightLegs { get; set; } = new List<Flight>();
        public bool MarkedAsIneligible { get; set; } = false;
        public bool HasBoardedBSM { get; set; } = false;
        public bool ScannedAtSmartGate { get; set; } = false;
        public bool HasBDXMessage { get; set; } = false;
        public string LastScannedLocation { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
           return obj is Passenger passenger &&
                   BookingID == passenger.BookingID &&
                   Bags.SequenceEqual(passenger.Bags) &&
                   FlightLegs.SequenceEqual(passenger.FlightLegs) &&
                   MarkedAsIneligible == passenger.MarkedAsIneligible &&
                   HasBoardedBSM == passenger.HasBoardedBSM &&
                   ScannedAtSmartGate == passenger.ScannedAtSmartGate &&
                   HasBDXMessage == passenger.HasBDXMessage;
        }
    }
}
