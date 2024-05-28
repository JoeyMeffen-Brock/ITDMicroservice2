using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using BrockSolutions.ITDService.Providers;

namespace BrockSolutions.ITDService.UnitTests
{
    public class ITDEligibilityCriteriaTests
    {
        [Fact]
        public void validRoute_isITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger validPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight validFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, validFlight, validRoute);
            Assert.True(result);
        }

        [Fact]
        public void ineligibleRoute_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger validPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight validFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route ineligibleRoute = new Route()
            {
                IsITDEligible = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, validFlight, ineligibleRoute);
            Assert.False(result);
        }

        [Fact]
        public void notDomesticConnection_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger validPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight notDomesticConnectionFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.International
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, notDomesticConnectionFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleCarrier_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger validPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight invalidCarrierFlight = new Flight()
            {
                Carrier = "VA",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, invalidCarrierFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void noCheckedBags_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger noBagPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 0,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight validFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(noBagPassenger, validFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleBCBP_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger badBCBPPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = true,
                HasIneligibleBSM = false
            };

            Flight validFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(badBCBPPassenger, validFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleBSM_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger badBSMPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = true
            };

            Flight validFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = true,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(badBSMPassenger, validFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleFlight_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger validPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 2,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = false
            };

            Flight ineligibleFlight = new Flight()
            {
                Carrier = "AC",
                IsITDEligible = false,
                Market = Flight.FlightMarket.Transborder
            };
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, ineligibleFlight, validRoute);
            Assert.False(result);
        }
    }
}