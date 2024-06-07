using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Data;

//using BrockSolutions.ITDService.Events;
using BrockSolutions.SmartSuite.Events;

namespace BrockSolutions.ITDService.Mappers
{
    public static class EligibilityMapper
    {
        //TODO: need a source for ITDEligibilityString
        public static ITDEligibilityInput MapToEligibilityInput(BagCreated bagCreatedEvent)
        {
            bool foundCheckedBag = bagCreatedEvent.BagFlightLegs.Any(x => 
                x.FlightDepartureStationCode == bagCreatedEvent.StationCode 
                && x.BagType == "CheckedBag");

            return new ITDEligibilityInput
            {

                FlightLegs = bagCreatedEvent.FlightLegs.ToList(),
                StationCode = bagCreatedEvent.StationCode,
                HasCheckedBag = foundCheckedBag,
                ITDEligibilityString = "ITDY", //bagCreatedEvent.ITDEligibilityString
            };
        }

        //TODO: either we need a way to get a specific station code, or we should just omit it as an input and do calculations for every flight leg
        public static ITDEligibilityInput MapToEligibilityInput(BagItineraryChanged bagItineraryChangedEvent)
        {
            return new ITDEligibilityInput
            {
                FlightLegs = bagItineraryChangedEvent.FlightLegs.ToList(),
                //StationCode = 
                HasCheckedBag = bagItineraryChangedEvent.BagFlightLegs.Any(x => x.BagType == "CheckedBag"),
                ITDEligibilityString = "ITDY", //bagItineraryChangedEvent.ITDEligibilityString
            };
        }

        //TODO: need flight legs and bag flight legs
        public static ITDEligibilityInput MapToEligibilityInput(BagPropertyChanged bagPropertyChangedEvent)
        {
            return new ITDEligibilityInput
            {
                //FlightLegs = bagItineraryChangedEvent.FlightLegs.ToList(),
                StationCode = bagPropertyChangedEvent.StationCode,
                //HasCheckedBag = bagItineraryChangedEvent.BagFlightLegs.Any(x => x.BagType == "CheckedBag"),
                ITDEligibilityString = "ITDY", //bagItineraryChangedEvent.ITDEligibilityString
            };
        }
    }
}
