using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.AdminSettings.DataService.DataService
{
    public class CommissionSettingsDataService
    {
        public string SaveCommissionSettings(Commission objCommission)
        {
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                if (!CheckExistCommissionSettings(CheckExistCommissionCondition(objCommission)))
                {
                    if (objCommission.CommissionId == 0)
                    {
                        sql = string.Format(
                                @"Insert Into Commission (SaleRepTypeId,SaleType,Amount,IsActive) 
                            Values({0},{1},{2},{3})", objCommission.SaleRepTypeId, objCommission.SaleTypeId, objCommission.ComissionAmount,objCommission.IsActive);
                    }
                    else
                    {
                        sql = string.Format(
                                @"Update Commission Set SaleRepTypeId={0},SaleType={1},Amount={2},IsActive={3} Where CommissionId={4}",
                                objCommission.SaleRepTypeId,objCommission.SaleTypeId, objCommission.ComissionAmount,objCommission.IsActive, objCommission.CommissionId);
                    }

                    connection.ExecuteNonQuery(sql);
                    rv = Operation.Success.ToString();
                }
                else
                {
                    rv = Operation.Exists.ToString();
                }
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

        private string CheckExistCommissionCondition(Commission objCommission)
        {
            string condition = "";
            if (objCommission.CommissionId == 0)
            {
                condition = string.Format(" Where SaleRepTypeId={0} And SaleType={1}", objCommission.SaleRepTypeId,objCommission.SaleTypeId);
            }
            else
            {
                condition = string.Format("Where SaleRepTypeId={0} And SaleType={1} And CommissionId !={2}",objCommission.SaleRepTypeId,objCommission.SaleTypeId,
                            objCommission.CommissionId);
            }

            return condition;
        }

        private bool CheckExistCommissionSettings(string condition)
        {
            string sql = string.Format(@" Select * From Commission {0}", condition);
            var data = Data<Commission>.DataSource(sql);
            return data.Count != 0 || data.Any();
        }

        public GridEntity<Commission> GetCommissionSettingsSummary(GridOptions options)
        {
            string sql = @"Select Commission.CommissionId,Commission.Amount As ComissionAmount,Commission.SaleType As SaleTypeId,Commission.SaleRepTypeId,Commission.IsActive,
            case when Commission.SaleType=1 then 'Installment Sale' else 'Cash Sale' end SaleTypeName, SalesRepTypeName  From Commission 
            left outer join SalesRepresentatorType ST on ST.SalesRepTypeId = Commission.SaleRepTypeId";
            var data = Kendo<Commission>.Grid.DataSource(options, sql, "SalesRepTypeName");
            return data;
        }

        public List<Commission> GetCommissionInfoBySaleRepType(int salesRepType)
        {
            string sql = @"Select Commission.CommissionId,Commission.Amount As ComissionAmount,Commission.SaleType As SaleTypeId From Commission Where IsActive=1 And SaleRepTypeId=" + salesRepType;
            var data = Data<Commission>.DataSource(sql);
            return data;
        }

        public CommissionAmount GetCommissionAmountBySaleRepType(int salesRepTypeId)
        {
            string sql = string.Format(@"Select tbl1.SaleRepTypeId,tbl1.Amount As InstallmentSaleCommission,tbl2.Amount As CashSaleCommission From (
            Select * From Commission Where SaleRepTypeId={0} and SaleType=1
            ) tbl1 left join (
            Select * From Commission Where SaleRepTypeId={0} and SaleType=2
            )tbl2 on tbl1.SaleRepTypeId=tbl2.SaleRepTypeId", salesRepTypeId);
            var data = Data<CommissionAmount>.DataSource(sql).SingleOrDefault();
            return data;
        }
    }
}
