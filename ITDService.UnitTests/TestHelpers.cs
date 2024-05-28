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
        public static ITDEligibilityProvider CreateProvider()
        {
            Mock<ILogger<ITDEligibilityProvider>> logger = new Mock<ILogger<ITDEligibilityProvider>>();
            Mock<IDapperQueryExecutor> queryExecutor = new Mock<IDapperQueryExecutor>();
            Mock<IOptionsMonitor<ITDServiceParameters>> parameters = new Mock<IOptionsMonitor<ITDServiceParameters>>();
            return new TestHarness(logger.Object, queryExecutor.Object, parameters.Object);
        }
        public class TestHarness : ITDEligibilityProvider
        {

            public TestHarness(ILogger<ITDEligibilityProvider> logger, IDapperQueryExecutor queryExecutor, IOptionsMonitor<ITDServiceParameters> parameters) : base(logger, queryExecutor, parameters)
            {
            }
        }
    }
}
