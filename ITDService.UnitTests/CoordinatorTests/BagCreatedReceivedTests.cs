using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Coordinators;
using BrockSolutions.SmartSuite.Events;

namespace BrockSolutions.ITDService.UnitTests
{
    public class BagCreatedReceivedTests
    {
        /*[Fact]
        public void bagCreatedForNewPassenger_createPassenger()
        {
            TestHelpers.ITDServiceCoordinatorTestHarness itdCoordinator = (TestHelpers.ITDServiceCoordinatorTestHarness) TestHelpers.CreateITDServiceCoordinator();
            var bagCreated = new BagCreated
            {
                BagId = 1,
                BookingId = 2
            };
            itdCoordinator.BagCreatedReceived(bagCreated);

            var expectedPassenger = new Passenger
            {
                BookingID = 2,
                //CheckedBagCount = 1,
                HasIneligibleBCBP = false,
                HasIneligibleBSM = true,
                HasBoardedBSM = false,
                ScannedAtSmartGate = false,
                HasBDXMessage = false
            };
            var newPassenger = itdCoordinator.RetrievePassengerForTesting(2);
            Assert.Equal(expectedPassenger, newPassenger);
        }*/
    }
}
