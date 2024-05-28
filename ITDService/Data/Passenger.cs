using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService
{
    public class Passenger
    {
        public int CheckedBagCount { get; set; }
        public bool HasIneligibleBCBP { get; set; } = false;
        public bool HasIneligibleBSM { get; set; } = false;
        public bool HasBoardedBSM { get; set; } = false;
        public bool ScannedAtSmartGate { get; set; }
        public bool HasBDXMessage { get; set; }

        public Passenger(int checkedBagCount, bool scannedAtSmartGate, bool hasBDXMessage)
        {
            CheckedBagCount = checkedBagCount;
            ScannedAtSmartGate = scannedAtSmartGate;
            HasBDXMessage = hasBDXMessage;
        }
    }
}
