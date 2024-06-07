using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Providers;

namespace BrockSolutions.ITDService.UnitTests
{
    public class BSMReceivedTests
    {
        /*[Fact]
        public void BSMReceived_noChanges()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                BookingID = 5,
                HasIneligibleBSM = false,
                HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM noChangeBSM = new BSM()
            {
                BookingID = 5,
                IsITDEligible = true
            };
            Passenger? unchangedPassenger = testProvider.UpdatePassengersFromBSM(noChangeBSM);
            Assert.Equal(initialPassenger, unchangedPassenger);
        }

        [Fact]
        public void passengerNotFound_createPassenger()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            BSM missingPassengerBSM = new BSM()
            {
                BookingID = 100
            };
            Passenger? unfoundPassenger = testProvider.UpdatePassengersFromBSM(missingPassengerBSM);
            Assert.Equal(missingPassengerBSM.BookingID, unfoundPassenger.BookingID);
        }

        [Fact]
        public void BSMIneligible_makePassengerIneligible()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                BookingID = 5,
                HasIneligibleBSM = false,
                HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM ineligibleBSM = new BSM()
            {
                BookingID = 5,
                IsITDEligible = false
            };
            Passenger? updatedPassenger = testProvider.UpdatePassengersFromBSM(ineligibleBSM);
            Assert.True(updatedPassenger.HasIneligibleBSM);
        }

        [Fact]
        public void receivedBoardedBSM_makePassengerBoarded()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                BookingID = 5,
                HasIneligibleBSM = false,
                HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM boardedBSM = new BSM()
            {
                BookingID = 5,
                BoardedBSM = true
            };
            Passenger? updatedPassenger = testProvider.UpdatePassengersFromBSM(boardedBSM);
            Assert.True(updatedPassenger.HasBoardedBSM);
        }*/
    }
}
