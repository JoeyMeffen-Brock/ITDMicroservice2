﻿using BrockSolutions.DapperWrapper;
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
            Passenger incompletePassenger = new Passenger(0, new List<BCBP>(), new List<BSM>(), false, false);

            bool result = provider.CheckIfPassengerIsITDComplete(incompletePassenger);
            Assert.False(result);
        }

        [Fact]
        public void passengerBoardedAtSmartGate_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger smartGateScannedPassenger = new Passenger(0, new List<BCBP>(), new List<BSM>(), true, false);

            bool result = provider.CheckIfPassengerIsITDComplete(smartGateScannedPassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBDXMessage_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            Passenger bdxMessagePassenger = new Passenger(0, new List<BCBP>(), new List<BSM>(), false, true);

            bool result = provider.CheckIfPassengerIsITDComplete(bdxMessagePassenger);
            Assert.True(result);
        }

        [Fact]
        public void passengerHasBoardedBSM_isITDComplete()
        {
            ITDEligibilityProvider provider = TestHelpers.CreateProvider();
            List<BSM> bsmsWithBoardedBSM = new List<BSM>()
            {
                new BSM()
                {
                    IsITDEligible = true,
                    BoardedBSM = true
                }
            };

            Passenger boardedBSMPassenger = new Passenger(0, new List<BCBP>(), bsmsWithBoardedBSM, false, false);

            bool result = provider.CheckIfPassengerIsITDComplete(boardedBSMPassenger);
            Assert.True(result);
        }
    }
}