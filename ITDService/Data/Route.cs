using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService.Data
{
    public record Route
    {
        public string originatingStationCode { get; set; } = "";
        public string destinationStationCode { get; set; } = "";
        public string airlineCode { get; set; } = "";
    }
}
