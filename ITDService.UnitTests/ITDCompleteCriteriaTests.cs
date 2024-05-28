using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using BrockSolutions.ITDService.Providers;

namespace BrockSolutions.ITDService.UnitTests
{
    public class ITDCompleteCriteriaTests
    {
        [Fact]
        public void noCriteriaMet_isNotITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger incompletePassenger = new Passenger(0, false, false);

            bool result = provider.CheckIfPassengerIsITDComplete(incompletePassenger);
            Assert.False(result);
        }

        [Fact]
        public void passengerBoardedAtSmartGate_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger smartGateScannedPassenger = new Passenger(0, true, false);

            bool result = provider.CheckIfPassengerIsITDComplete(smartGateScannedPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBDXMessage_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger bdxMessagePassenger = new Passenger(0, false, true);

            bool result = provider.CheckIfPassengerIsITDComplete(bdxMessagePassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBoardedBSM_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger boardedBSMPassenger = new Passenger(0, false, false);
            boardedBSMPassenger.HasBoardedBSM = true;

            bool result = provider.CheckIfPassengerIsITDComplete(boardedBSMPassenger);
            Assert.True(result);
        }
    }
}
