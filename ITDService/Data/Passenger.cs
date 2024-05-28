using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService
{
    public class Passenger
    {
        public int PassengerID { get; set; } = -1;
        public int CheckedBagCount { get; set; } = 0;
        public bool HasIneligibleBCBP { get; set; } = false;
        public bool HasIneligibleBSM { get; set; } = false;
        public bool HasBoardedBSM { get; set; } = false;
        public bool ScannedAtSmartGate { get; set; } = false;
        public bool HasBDXMessage { get; set; } = false;

        public Passenger()
        {

        }

        public Passenger(int passengerID, int checkedBagCount, bool scannedAtSmartGate, bool hasBDXMessage)
        {
            PassengerID = passengerID;
            CheckedBagCount = checkedBagCount;
            ScannedAtSmartGate = scannedAtSmartGate;
            HasBDXMessage = hasBDXMessage;
        }
    }
}
