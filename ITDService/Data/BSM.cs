using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService
{
    public class BSM
    {
        public int PassengerID { get; set; } = -1;
        public bool IsITDEligible { get; set; } = false;
        public bool BoardedBSM { get; set; } = false;
    }
}
