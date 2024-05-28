using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using BrockSolutions.ITDService.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BrockSolutions.ITDService.UnitTests
{
    public static class TestHelpers
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
            }
        }

        public static PassengerProvider CreatePassengerProvider()
        {
            Mock<ILogger<PassengerProvider>> logger = new Mock<ILogger<PassengerProvider>>();
            Mock<IDapperQueryExecutor> queryExecutor = new Mock<IDapperQueryExecutor>();
            Mock<IOptionsMonitor<ITDServiceParameters>> parameters = new Mock<IOptionsMonitor<ITDServiceParameters>>();
            return new PassengerTestHarness(logger.Object, queryExecutor.Object, parameters.Object);
        }

        public class PassengerTestHarness : PassengerProvider
        {
            private List<Passenger> _fakeDatabase = new List<Passenger>();

            public PassengerTestHarness(ILogger<PassengerProvider> logger, IDapperQueryExecutor queryExecutor, IOptionsMonitor<ITDServiceParameters> parameters) : base(logger, queryExecutor, parameters)
            {
            }

            public override Passenger? GetPassengerByID(int passengerID)
            {
                return _fakeDatabase.FirstOrDefault(p => p.PassengerID == passengerID);
            }

            public override bool AddPassengerToDatabase(Passenger passenger)
            {
                _fakeDatabase.Add(passenger);
                return true;
            }
        }
    }
}
