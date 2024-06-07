using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrockSolutions.ITDService.Data
{
    public record Bag
    {
        public long BagId { get; set; } = 0;
        public bool CheckedBag { get; set; } = false;
    }
}
