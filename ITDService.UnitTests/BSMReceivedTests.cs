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
        [Fact]
        public void BSMReceived_noChanges()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                PassengerID = 5,
                HasIneligibleBSM = false,
                 HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM noChangeBSM = new BSM()
            {
                PassengerID = 5,
                IsITDEligible = true
            };
            Passenger? unchangedPassenger = testProvider.UpdatePassengersFromBSM(noChangeBSM);
            Assert.Equal(initialPassenger, unchangedPassenger);
        }

        [Fact]
        public void passengerNotFound_returnNull()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            BSM missingPassengerBSM = new BSM()
            {
                PassengerID = 100
            };
            Passenger? unfoundPassenger = testProvider.UpdatePassengersFromBSM(missingPassengerBSM);
            Assert.Null(unfoundPassenger);
        }

        [Fact]
        public void BSMIneligible_makePassengerIneligible()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                PassengerID = 5,
                HasIneligibleBSM = false,
                HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM ineligibleBSM = new BSM()
            {
                PassengerID = 5,
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
                PassengerID = 5,
                HasIneligibleBSM = false,
                HasBoardedBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BSM boardedBSM = new BSM()
            {
                PassengerID = 5,
                BoardedBSM = true
            };
            Passenger? updatedPassenger = testProvider.UpdatePassengersFromBSM(boardedBSM);
            Assert.True(updatedPassenger.HasBoardedBSM);
        }
    }
}
