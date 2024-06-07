using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Providers;

namespace BrockSolutions.ITDService.UnitTests
{
    public class BCBPReceivedTests
    {
        /*[Fact]
        public void BCBPReceived_noChanges()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                BookingID = 5
            };
            //testProvider.AddPassengerToDatabase(initialPassenger);

            BCBP noChangeBCBP = new BCBP()
            {
                PassengerID = 5
            };
            Passenger? unchangedPassenger = testProvider.UpdatePassengersFromBCBP(noChangeBCBP);
            Assert.Equal(initialPassenger, unchangedPassenger);
        }

        [Fact]
        public void passengerNotFound_returnNull()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            BCBP missingPassengerBCBP = new BCBP()
            {
                PassengerID = 5
            };
            Passenger? notFoundPassenger = testProvider.UpdatePassengersFromBCBP(missingPassengerBCBP);
            Assert.Null(notFoundPassenger);
        }

        [Fact]
        public void BCBPIneligible_makePassengerIneligible()
        {
            PassengerProvider testProvider = TestHelpers.CreatePassengerProvider();

            Passenger initialPassenger = new Passenger()
            {
                BookingID = 5,
                HasIneligibleBSM = false
            };
            testProvider.AddPassengerToDatabase(initialPassenger);

            BCBP ineligibleBCBP = new BCBP()
            {
                PassengerID = 5,
                IsITDEligible = false
            };
            Passenger? updatedPassenger = testProvider.UpdatePassengersFromBCBP(ineligibleBCBP);
            Assert.True(updatedPassenger.HasIneligibleBCBP);
        }*/
    }
}
