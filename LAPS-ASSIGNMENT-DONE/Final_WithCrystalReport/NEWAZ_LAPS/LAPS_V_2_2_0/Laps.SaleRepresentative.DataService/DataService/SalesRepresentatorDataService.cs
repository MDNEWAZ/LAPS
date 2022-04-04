using System;
using System.Collections.Generic;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.SaleRepresentative.DataService.DataService
{
    public class SalesRepresentatorDataService
    {
       
        public string SaveSalesRepresentator(SalesRepresentator objSalesRepresentator)
        {
            string rv = "";

            CommonConnection connection = new CommonConnection();
            try
            {
                string condition = "";

                if (objSalesRepresentator != null && objSalesRepresentator.Id == 0)
                {
                    condition = string.Format(@" Where SalesRepId='{0}'", objSalesRepresentator.SalesRepId.Trim());  //Previous repplace SalesRepCode BY SalesRepId 
                }
                else
                {
                    if (objSalesRepresentator != null)
                        condition = string.Format(@" Where SalesRepId='{0}' And ID != {1}", objSalesRepresentator.SalesRepId.Trim(), objSalesRepresentator.Id);   //Previous repplace SalesRepCode BY SalesRepId 
                }

                if (CheckExistSalesRepCode(condition))
                {
                    return "Exist";
                }

                string sql = "";
                if (objSalesRepresentator != null && objSalesRepresentator.Id == 0)
                {

                    sql =
                        string.Format(
                            @"Insert into SalesRepresentator(SalesRepId,SalesRepType,SalesRepCode,Address,SalesRepSMSMobNo,
                             SalesRepBkashNo,FixedAmount,CompanyId,BranchId,IsActive,IsIncentiveActive,IsCommissionActive,IsSalesRepSmsSent) Values('{0}',{1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12})",
                            objSalesRepresentator.SalesRepId,
                            objSalesRepresentator.SalesRepType, objSalesRepresentator.SalesRepCode,
                            objSalesRepresentator.Address,
                            objSalesRepresentator.SalesRepSmsMobNo, objSalesRepresentator.SalesRepBkashNo,
                            objSalesRepresentator.FixedAmount,
                            objSalesRepresentator.CompanyId, objSalesRepresentator.BranchId,
                            objSalesRepresentator.IsActive,objSalesRepresentator.IsIncentiveActive,objSalesRepresentator.IsCommissionActive,objSalesRepresentator.IsSalesRepSmsSent);


                }
                else
                {
                    sql = string.Format(@" Update SalesRepresentator Set SalesRepId='{0}',SalesRepType={1},SalesRepCode='{2}',Address='{3}',SalesRepSMSMobNo='{4}',
                    SalesRepBkashNo='{5}',FixedAmount={6},CompanyId={7},BranchId={8},IsActive={9},IsIncentiveActive={10},IsCommissionActive={11},IsSalesRepSmsSent = {12} Where Id={13}", objSalesRepresentator.SalesRepId,
                    objSalesRepresentator.SalesRepType, objSalesRepresentator.SalesRepCode, objSalesRepresentator.Address,
                    objSalesRepresentator.SalesRepSmsMobNo, objSalesRepresentator.SalesRepBkashNo, objSalesRepresentator.FixedAmount,
                    objSalesRepresentator.CompanyId, objSalesRepresentator.BranchId, objSalesRepresentator.IsActive,objSalesRepresentator.IsIncentiveActive,objSalesRepresentator.IsCommissionActive,objSalesRepresentator.IsSalesRepSmsSent, objSalesRepresentator.Id);


                }
                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
            }
            catch (Exception exception)
            {

                rv = exception.Message;

            }
            finally
            {
                connection.Close();
            }

            return rv;
        }

        private bool CheckExistSalesRepCode(string condition)
        {

            string sql = string.Format(@"Select * From SalesRepresentator {0}", condition);
            var data = Data<SalesRepresentator>.DataSource(sql);
            return data.Count == 0 ? false : true;
        }

        public GridEntity<SalesRepresentator> GetAllSalesRepresentator(GridOptions options)
        {
            string sql = string.Format(@"Select SalesRepresentator.*,CompanyName,BRANCHNAME,SalesRepresentatorType.SalesRepTypeName From SalesRepresentator
                left outer join Company on Company.CompanyId=SalesRepresentator.CompanyId
                left outer join Branch on Branch.BRANCHID=SalesRepresentator.BranchId
                left outer join SalesRepresentatorType on  SalesRepresentatorType.SalesRepTypeId=SalesRepresentator.SalesRepType");
            var data = Kendo<SalesRepresentator>.Grid.DataSource(options, sql, "Id");
            return data;
        }

        public List<SalesRepresentatorType> GetSalesRepresentatorType()
        {
            string sql = string.Format(@"Select * From SalesRepresentatorType Where IsActive=1");
            return Kendo<SalesRepresentatorType>.Combo.DataSource(sql);
        }

        public List<SalesRepresentator> GetAllSalesRepresentatorCombo(string condition)
        {
            string sql = string.Format(@"Select * From SalesRepresentator Where IsActive=1 {0}", condition);
            var data = Kendo<SalesRepresentator>.Combo.DataSource(sql);
            return data;
        }

        public List<SalesRepresentator> GetAllSalesRepresentatorCombo()
        {
            string sql = string.Format(@"Select * From SalesRepresentator Where IsActive=1");
            var data = Kendo<SalesRepresentator>.Combo.DataSource(sql);
            return data;
        }

        public SalesRepresentator GetAllSalesRepresentatorById(string salesRepId)
        {
            string sql = string.Format(@"Select SalesRepresentator.*,SalesRepTypeName From SalesRepresentator
            left outer join SalesRepresentatorType on SalesRepresentatorType.SalesRepTypeId=SalesRepresentator.SalesRepType Where SalesRepId='{0}'", salesRepId);
            var data = Data<SalesRepresentator>.DataSource(sql).SingleOrDefault();
            return data;
        }

        public List<SalesRepresentator> GetSalesRepresentatorByCompanyAndBranch(string condition)
        {
            string sql = string.Format(@"Select SalesRepresentator.*,SalesRepTypeName From SalesRepresentator
            left join SalesRepresentatorType on SalesRepresentatorType.SalesRepTypeId=SalesRepresentator.SalesRepType Where SalesRepresentator.IsActive=1 {0}", condition);
            var data = Kendo<SalesRepresentator>.Combo.DataSource(sql);
            return data;
        }

        public List<SalesRepresentator> GetSalesRepComboByCompanyBranchAndType(string condition)
        {
            string sql = string.Format(@"Select SalesRepresentator.*,SalesRepTypeName From SalesRepresentator
                left join SalesRepresentatorType on SalesRepresentatorType.SalesRepTypeId=SalesRepresentator.SalesRepType Where SalesRepresentator.IsActive=1 {0}", condition);
            var data = Kendo<SalesRepresentator>.Combo.DataSource(sql);
            return data;
        }
    }
}
