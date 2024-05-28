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
        public List<BCBP> TrackedBCBPs { get; set; }
        public List<BSM> TrackedBSMs { get; set; }
        public bool ScannedAtSmartGate { get; set; }
        public bool HasBDXMessage { get; set; }

        public Passenger(int checkedBagCount, List<BCBP> trackedBCBPs, List<BSM> trackedBSMs, bool scannedAtSmartGate, bool hasBDXMessage)
        {
            CheckedBagCount = checkedBagCount;
            TrackedBCBPs = trackedBCBPs;
            TrackedBSMs = trackedBSMs;
            ScannedAtSmartGate = scannedAtSmartGate;
            HasBDXMessage = hasBDXMessage;
        }
    }
}
