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
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger incompletePassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 0,
            };

            bool result = provider.CheckIfPassengerIsITDComplete(incompletePassenger);
            Assert.False(result);
        }

        [Fact]
        public void passengerScannedAtSmartGate_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger smartGateScannedPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 0,
                ScannedAtSmartGate = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(smartGateScannedPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBDXMessage_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger bdxMessagePassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 0,
                HasBDXMessage = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(bdxMessagePassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBoardedBSM_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger boardedBSMPassenger = new Passenger()
            {
                PassengerID = 0,
                CheckedBagCount = 0,
                HasBoardedBSM = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(boardedBSMPassenger);
            Assert.True(result);
        }
    }
}
