using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
    public class ReplacementCs
    {
        public int ReplacementCsId { get; set; }
        public int BranchId { get; set; }
        public int ModelId { get; set; }
        public int NoOfUnit { get; set; }
        public DateTime ReplacementDate { get; set; }
    }
}
