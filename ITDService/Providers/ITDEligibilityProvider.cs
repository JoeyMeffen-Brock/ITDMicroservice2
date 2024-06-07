using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Data;
using BrockSolutions.ITDService.Options;
using BrockSolutions.SmartSuite.Events;
using Microsoft.Extensions.Options;

namespace BrockSolutions.ITDService.Providers
{
    public class ITDEligibilityProvider : IExampleProvider
    {
        private readonly IDapperQueryExecutor _dapperQueryExecutor;
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<ITDServiceParameters> _parameters;

        protected static List<string> _eligibleCarriers = new List<string> { };

        protected static List<Route> _eligibleRoutes = new List<Route> { };

        protected static List<Flight> _eligibleFlights = new List<Flight> { };

        //TODO: Probably need more details than just a string, replace with a real location type later when API is more fleshed out
        protected static List<string> _postITDLocations = new List<string> { };

        protected IStateProvider _stateProvider;

        public ITDEligibilityProvider
        (
            ILogger<ITDEligibilityProvider> logger,
            IDapperQueryExecutor queryExecutor,
            IOptionsMonitor<ITDServiceParameters> parameters
        )
        {
            _dapperQueryExecutor = queryExecutor;
            _logger = logger;
            _parameters = parameters;
            _stateProvider = new JSONStateProvider();
        }

        //TODO: Add actual logic here when we have access to the flight market of the outbound leg
        public bool IsDomesticFlight(Flight flightLeg)
        {
            return flightLeg.Market == Flight.FlightMarket.Domestic;
        }

        public bool IsRouteITDEligible(string prevStationCode, string currentStationCode, string airline)
        {
            return _eligibleRoutes.Any(route => route.originatingStationCode == prevStationCode 
                && route.destinationStationCode == currentStationCode 
                && route.airlineCode == airline);
        }

        public bool IsOutboundFlightITDEligible(Flight outboundFlightLeg)
        {
            return _eligibleFlights.Any(flight => flight.DepartureDateLocal == outboundFlightLeg.DepartureDateLocal 
                && flight.FlightNumber == outboundFlightLeg.FlightNumber && flight.CarrierCode == outboundFlightLeg.CarrierCode);
        }

        public bool CheckIfPassengerisITDEligible(Passenger passenger, string stationCode)
        {
            var outboundLeg = passenger.FlightLegs.FirstOrDefault(x => x.DepartureStation == stationCode);
            var inboundLeg = passenger.FlightLegs.FirstOrDefault(x => x.ArrivalStation == stationCode);

            if (outboundLeg == null || inboundLeg == null)
            {
                return false;
            }

            if (!_eligibleCarriers.Contains(outboundLeg.CarrierCode))
            {
                return false;
            }

            if (!IsDomesticFlight(outboundLeg))
            {
                return false;
            }

            if (!IsRouteITDEligible(inboundLeg.DepartureStation, inboundLeg.ArrivalStation, inboundLeg.CarrierCode))
            {
                return false;
            }

            if (!IsOutboundFlightITDEligible(outboundLeg))
            {
                return false;
            }

            if (!passenger.Bags.Any(bag => bag.CheckedBag == true))
            {
                return false;
            }

            //TODO: iron out the logic here; might be that we should have different return values depending on if it equals ITDY/ITDE vs if it doesn't match any value
            if (passenger.MarkedAsIneligible)
            {
                return false;
            }

            return true;
        }

        public bool CheckIfPassengerIsITDComplete(Passenger passenger)
        {
            if (passenger.ScannedAtSmartGate)
            {
                return true;
            }

            if (passenger.HasBDXMessage)
            {
                return true;
            }

            if (passenger.HasBoardedBSM)
            {
                return true;
            }

            if (_postITDLocations.Contains(passenger.LastScannedLocation))
            {
                return true;
            }

            return false;
        }
    }



    public interface IExampleProvider
    {

    }
}
