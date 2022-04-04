using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditTrail.Entity
{
   public class AuditTrails
    {
       public string Requested_Url { get; set; }
       public int Audit_Id { get; set; }
       public int User_Id { get; set; }
       public string Client_User { get; set; }
       public string Client_Ip { get; set; }
       public string Shortdescription { get; set; }
       public string AudiT_Type { get; set; }
       public string Audit_Description { get; set; }
       public DateTime Action_Date { get; set; }


    }
}
