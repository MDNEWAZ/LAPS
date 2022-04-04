using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azolution.Entities.Sale
{
   public class ReleaseLisenceGenerate
    {
       public string SInvoice { get; set; }
       public int Status { get; set; }
       public string Number { get; set; }
       public DateTime DueDate { get; set; }
       public int SaleId { get; set; }
       public int CustomerId { get; set; }
       public string Name { get; set; }
       public string Phone { get; set; }
       public string Phone2 { get; set; }
       public int VarificationType { get; set; }
       public string CustomerCode { get; set; }

       public string BranchCode { get; set; }
       public int IIM { get; set; }
       public int IsCustomerUpgraded { get; set; }
       public string BranchSmsMobileNumber { get; set; }
       public int IsSmsEligible { get; set; }
    }
}
