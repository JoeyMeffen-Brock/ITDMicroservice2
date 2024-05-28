using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;

namespace BrockSolutions.ITDService.Providers
{
    public class ITDEligibilityProvider : IExampleProvider
    {
        // this class is used to show how to setup a provider with a DapperQueryExecutor injected from startup.
        // see references to IDapperQueryExecutor for registration example
        private readonly IDapperQueryExecutor _dapperQueryExecutor;
        private readonly ILogger _logger;
        private readonly IOptionsMonitor<ITDServiceParameters> _parameters;

        private static readonly List<string> ELIGIBLE_CARRIERS = new List<string> { "AC", "QK", "RV" };

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
        }

        public bool CheckIfPassengerisITDEligible(Passenger passenger, Flight flight, Route route)
        {
            if (!route.IsITDEligible)
            {
                return false;
            }

            if (!flight.IsITDEligible)
            {
                return false;
            }

            if (!ELIGIBLE_CARRIERS.Contains(flight.Carrier))
            {
                return false;
            }

            if (passenger.CheckedBagCount == 0)
            {
                return false;
            }

            if (passenger.HasIneligibleBCBP)
            {
                return false;
            }

            if (passenger.HasIneligibleBSM)
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

            return false;
        }
    }



    public interface IExampleProvider
    {

    }
}
