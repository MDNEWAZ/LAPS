using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class RatingConfigurationDataService
    {
        public GridEntity<Due> GetACompayDueKpi(GridOptions options, string companyId)
        {
            try
            {
                var query = string.Format("Select SD.DueId, SD.FromDue,SD.ToDue, SD.TypeId, AT.Type as Color, SD.EntryDate, SD.Status, C.CompanyId, C.CompanyName  from Sale_Due SD " +
                                          "LEFT JOIN Company C ON C.CompanyId=SD.CompanyId LEFT JOIN (Select * from Sale_AllType AT Where AT.Flag='Color') AT ON AT.TypeId=SD.TypeId " +
                                          "Where C.IsActive={0} AND C.CompanyId in ({1})", 1, companyId);
                return Kendo<Due>.Grid.GenericDataSource(options, query, "CompanyId");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveDue(Due aDue, int userId)
        {
            var aConnection = new CommonConnection();
            string seccess;
            try
            {
                string query;
                var entrydate = DateTime.Now.ToString("dd-MMM-yyyy");
                var updatedate = DateTime.Now.ToString("dd-MMM-yyyy");
                if (aDue.DueId == 0)
                {
                    query = string.Format(
                        @"INSERT INTO dbo.[Sale_Due] VALUES({0},{1},{2},{3},{4},'{5}','{6}',{7},{8},{9})",
                        aDue.ACompany.CompanyId, aDue.FromDue, aDue.ToDue, aDue.AAllType.TypeId, aDue.Status, entrydate, "", 0, userId, 0);
                }
                else
                {
                    query = string.Format("UPDATE [dbo].[Sale_Due] SET [FromDue] = {0},[ToDue] = {1},[Status]={2},[Updated] = '{3}',[Flag] = {4},[UpdateBy]={5},[TypeId]={6} Where CompanyId={7} And DueId={8}",
                             aDue.FromDue, aDue.ToDue, aDue.Status, updatedate, 0, userId, aDue.AAllType.TypeId, aDue.ACompany.CompanyId, aDue.DueId);
                }
                aConnection.ExecuteNonQuery(query);
                aConnection.Close();
                seccess = Operation.Success.ToString();
            }
            catch (Exception ex)
            {
                aConnection.Close();
                seccess = Operation.Failed.ToString();
            }
            return seccess;
        }
    }
}
