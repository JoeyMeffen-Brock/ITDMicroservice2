using BrockSolutions.DapperWrapper;
using BrockSolutions.ITDService.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using BrockSolutions.ITDService.Providers;
using BrockSolutions.ITDService.Data;

namespace BrockSolutions.ITDService.UnitTests.ProviderTests
{
    public class ITDCompleteCriteriaTests
    {
        [Fact]
        public void noCriteriaMet_isNotITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger incompletePassenger = new Passenger();

            bool result = provider.CheckIfPassengerIsITDComplete(incompletePassenger);
            Assert.False(result);
        }

        [Fact]
        public void passengerScannedAtSmartGate_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger smartGateScannedPassenger = new Passenger()
            {
                ScannedAtSmartGate = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(smartGateScannedPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBDXMessage_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger bdxScannedPassenger = new Passenger()
            {
                HasBDXMessage = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(bdxScannedPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBoardedBSM_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger boardedBSMPassenger = new Passenger()
            {
                HasBoardedBSM = true
            };

            bool result = provider.CheckIfPassengerIsITDComplete(boardedBSMPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasTrackingScanAtPostITDLocation_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger postITDPassenger = new Passenger()
            {
                LastScannedLocation = "Post ITD Location"
            };

            bool result = provider.CheckIfPassengerIsITDComplete(postITDPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasTrackingScanAtNonPostITDLocation_isNotITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateITDProvider();
            Passenger nonPostITDPassenger = new Passenger()
            {
                LastScannedLocation = "Not Post ITD Location"
            };

            bool result = provider.CheckIfPassengerIsITDComplete(nonPostITDPassenger);
            Assert.False(result);
        }
    }
}
