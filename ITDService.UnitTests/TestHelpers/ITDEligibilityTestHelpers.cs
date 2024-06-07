using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Coordinators;
using BrockSolutions.ITDService.Data;
using BrockSolutions.ITDService.Options;
using BrockSolutions.ITDService.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BrockSolutions.ITDService.UnitTests.TestHelpers
{
    public static class ITDEligibilityTestHelpers
    {
        public static ITDEligibilityProvider CreateITDProvider()
        {
            Mock<ILogger<ITDEligibilityProvider>> logger = new Mock<ILogger<ITDEligibilityProvider>>();
            Mock<IDapperQueryExecutor> queryExecutor = new Mock<IDapperQueryExecutor>();
            Mock<IOptionsMonitor<ITDServiceParameters>> parameters = new Mock<IOptionsMonitor<ITDServiceParameters>>();
            return new ITDEligibilityTestHarness(logger.Object, queryExecutor.Object, parameters.Object);
        }

        public class ITDEligibilityTestHarness : ITDEligibilityProvider
        {
            public ITDEligibilityTestHarness(ILogger<ITDEligibilityProvider> logger, IDapperQueryExecutor queryExecutor, IOptionsMonitor<ITDServiceParameters> parameters) : base(logger, queryExecutor, parameters)
            {
                _eligibleCarriers = new List<string> { "AC", "QK", "RV" };

                _eligibleRoutes = new List<Route> {
                    new Route()
                    {
                        originatingStationCode = "LAX",
                        destinationStationCode = "YYZ",
                        airlineCode = "AC",
                    }
                };

                _eligibleFlights = new List<Flight>
                {
                    new Flight()
                    {
                        DepartureDateLocal = new DateTime(2022, 1, 1).Ticks,
                        FlightNumber = "777",
                        CarrierCode = "AC"
                    }
                };

                //TODO: Probably need more details than just a string, replace with a real location type later when API is more fleshed out
                _postITDLocations = new List<string> { "Post ITD Location", };
            }
        }
    }
}
