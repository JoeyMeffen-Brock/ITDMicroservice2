using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BrockSolutions.ITDService.Events;
using BrockSolutions.ITDService.Providers;
using BrockSolutions.SmartSuite.Events;
using NLog.Fluent;

namespace BrockSolutions.ITDService.Coordinators
{
    public class ITDServiceCoordinator
    {
        public enum ITDStatus
        {
            Ineligible,
            Eligible,
            Complete
        }

        private readonly ILogger _logger;
        protected readonly ITDEligibilityProvider _itdEligibilityProvider;
        protected readonly PassengerProvider _passengerProvider;

        public ITDServiceCoordinator(ITDEligibilityProvider itdEligibilityProvider, PassengerProvider passengerProvider, ILogger log)
        {
            _itdEligibilityProvider = itdEligibilityProvider;
            _passengerProvider = passengerProvider;
            _logger = log;
        }


        public void BagCreatedReceived(BagCreated bagCreatedEvent)
        {
            var passenger = _passengerProvider.UpdatePassengerFromBagCreated(bagCreatedEvent);

            bool isITDEligible = _itdEligibilityProvider.CheckIfPassengerisITDEligible(passenger, bagCreatedEvent.StationCode);

            if (isITDEligible)
            {
                _logger.Log(LogLevel.Information, "Passenger is ITD Eligible");
            } else
            {
                _logger.Log(LogLevel.Information, "Passenger is not ITD Eligible");
            }
        }

        public void BagItineraryChangedReceived(BagItineraryChanged itineraryChangedEvent)
        {
            var passenger = _passengerProvider.UpdatePassengerFromBagItineraryChanged(itineraryChangedEvent);

            //TODO: need to get the station code from somewhere
            bool isITDEligible = _itdEligibilityProvider.CheckIfPassengerisITDEligible(passenger, "YYZ");

            if (isITDEligible)
            {
                _logger.Log(LogLevel.Information, "Passenger is ITD Eligible");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Passenger is not ITD Eligible");
            }
        }

        public void BagPropertyChangedReceived(BagPropertyChanged propertyChangedEvent)
        {
            var passenger = _passengerProvider.UpdatePassengerFromBagPropertyChanged(propertyChangedEvent);
            if (passenger == null)
            {
                _logger.Log(LogLevel.Information, "Passenger not found");
                return;
            }

            //TODO: need to get the station code from somewhere
            bool isITDEligible = _itdEligibilityProvider.CheckIfPassengerisITDEligible(passenger, "YYZ");

            if (isITDEligible)
            {
                _logger.Log(LogLevel.Information, "Passenger is ITD Eligible");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Passenger is not ITD Eligible");
            }
        }
    }
}
