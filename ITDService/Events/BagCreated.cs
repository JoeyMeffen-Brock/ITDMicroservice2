using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService.Events
{
    public class BagCreated
    {
        public int BagTagID { get; set; } = -1;
        public int BookingID { get; set; } = -1;
        public bool IsITDEligible { get; set; } = false;
        public bool BoardedBSM { get; set; } = false;
    }
}
