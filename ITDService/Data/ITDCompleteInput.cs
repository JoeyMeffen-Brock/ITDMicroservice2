using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService.Data
{
    public record ITDCompleteInput
    {
        public enum SourceType { 
            None,
            BoardedAtSmartGate,
            BDXScanned,
            BoardedBSM,
            PassengerTrackingScan
        }

        public SourceType Source { get; set; } = SourceType.None;
        public string ScanLocation { get; set; } = "";
    }
}
