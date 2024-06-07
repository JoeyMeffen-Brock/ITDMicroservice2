using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrockSolutions.ITDService.Coordinators;
using BrockSolutions.ITDService.Events;

namespace BrockSolutions.ITDService.UnitTests
{
    public class BagCreatedReceivedTests
    {
        [Fact]
        public void bagCreatedForNewPassenger_CreatePassenger()
        {
            var bagCreated = new BagCreated
            {
                BagTagID = 1,
                BookingID = 2,
                IsITDEligible = true,
                BoardedBSM = false
            };
        }
    }
}
