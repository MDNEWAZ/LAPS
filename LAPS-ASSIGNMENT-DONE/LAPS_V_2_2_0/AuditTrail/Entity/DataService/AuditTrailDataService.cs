using System;
using Utilities;

namespace AuditTrail.Entity.DataService
{
    public class AuditTrailDataService
    {
       
        public AuditTrailDataService()
        {
            //audit=new AuditTrails();
            //audit.Shortdescription = title;
            //audit.User_Id = userId;
            //audit.Audit_Description = statement;

        }
         public bool SendAudit(AuditTrails auditTrail)
         {

            
             if (auditTrail != null)
             {
                 auditTrail.Action_Date = DateTime.Now;
                 auditTrail.Audit_Description =auditTrail.Audit_Description==null?"":auditTrail.Audit_Description.Replace("'", "\"");
                 return ExecuteAudit(auditTrail);
             }
             else
             {
                 return false;
             }

         }
        private bool ExecuteAudit(AuditTrails sAuditTrail)
        {
            bool rv = false;
            var connection = new CommonConnection();
            var activityDate = DateFormatter.DateTimeForQuery(sAuditTrail.Action_Date, connection.DatabaseType);
            try
            {
                string sql = "";
//                if (connection.DatabaseType == DatabaseType.Oracle)
//                {
//                     sql = string.Format(@"Insert Into Audit_Trail (User_Id, Client_User, 
//                               Client_Ip, Shortdescription, AUDIT_TYPE, 
//                               Audit_Description, Action_Date,Requested_Url)                                
//                            VALUES ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')",
//                           sAuditTrail.User_Id, sAuditTrail.Client_User, sAuditTrail.Client_Ip, sAuditTrail.Shortdescription, sAuditTrail.AudiT_Type, sAuditTrail.Audit_Description, "to_date('" + sAuditTrail.Action_Date.ToString("dd/MM/yyyy HH:mm:ss")+"'" + ",'DD/MM/YYYY  hh24:mi:ss')",sAuditTrail.Requested_Url);
  
//                }
//                else 
//                {
                    sql = string.Format(@"Insert Into Audit_Trail (User_Id, Client_User, 
                               Client_Ip, Shortdescription, AUDIT_TYPE, 
                               Audit_Description, Action_Date,Requested_Url)                                
                            VALUES ({0},'{1}','{2}','{3}','{4}','{5}',{6},'{7}')",
                          sAuditTrail.User_Id, sAuditTrail.Client_User, sAuditTrail.Client_Ip, sAuditTrail.Shortdescription, sAuditTrail.AudiT_Type, sAuditTrail.Audit_Description, activityDate, sAuditTrail.Requested_Url);
        
                //}
                  
                connection.ExecuteNonQuery(sql);

                rv =true;
            }
            catch (Exception)
            {
                rv = false;

            }
            finally
            {
                connection.Close();
            }
            return rv;


        }


      
    }
}
