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
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger validPassenger = new Passenger(2, validBCBPs, validBSMs, false, false);

            Flight validFlight = new Flight("AC", true);
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
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger validPassenger = new Passenger(2, validBCBPs, validBSMs, false, false);

            Flight validFlight = new Flight("AC", true);
            Route ineligibleRoute = new Route()
            {
                IsITDEligible = false
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, validFlight, ineligibleRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleCarrier_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger validPassenger = new Passenger(2, validBCBPs, validBSMs, false, false);

            Flight invalidCarrierFlight = new Flight("VA", true);
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
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger noBagPassenger = new Passenger(0, validBCBPs, validBSMs, false, false);

            Flight validFlight = new Flight("AC", true);
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
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> ineligibleBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = false
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger badBCBPPassenger = new Passenger(2, ineligibleBCBPs, validBSMs, false, false);

            Flight validFlight = new Flight("AC", true);
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
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> ineligibleBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = false
                }
            };
            Passenger badBCBPPassenger = new Passenger(2, validBCBPs, ineligibleBSMs, false, false);

            Flight validFlight = new Flight("AC", true);
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(badBCBPPassenger, validFlight, validRoute);
            Assert.False(result);
        }

        [Fact]
        public void ineligibleFlight_notITDEligible()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BCBP> validBCBPs = new List<BCBP>()
            {
                new BCBP()
                {
                    IsITDEligible = true
                }
            };
            List<BSM> validBSMs = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true
                }
            };
            Passenger validPassenger = new Passenger(2, validBCBPs, validBSMs, false, false);

            Flight ineligibleFlight = new Flight("AC", false);
            Route validRoute = new Route()
            {
                IsITDEligible = true
            };

            bool result = provider.CheckIfPassengerisITDEligible(validPassenger, ineligibleFlight, validRoute);
            Assert.False(result);
        }
    }
}