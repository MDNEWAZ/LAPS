using System;
using System.Collections.Generic;
using System.Data;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.DataService.DataService
{
    public class BankBranchDataService
    {
        public string SaveBank(Bank bankObj)
        {
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                string condition = "";
                if (bankObj.BankId == 0)
                {
                    condition = string.Format(" Where BankName='{0}' or BankCode={1}", bankObj.BankName,bankObj.BankCode);
                }
                else
                {
                    condition = string.Format(@" Where BankId !={0} And (BankName='{1}' or BankCode={2})", bankObj.BankId, bankObj.BankName,bankObj.BankCode);
                }

                var res = GetExistBank(condition, connection);

                if(res==0)
                {
                    if (bankObj.BankId == 0)
                    {
                        sql = string.Format(@"Insert Into Bank(BankCode,BankName,IsActive) Values({0},'{1}',{2})", bankObj.BankCode, bankObj.BankName, bankObj.IsActive);

                    }
                    else
                    {
                        sql = string.Format(@"Update Bank Set BankCode={0},BankName='{1}',IsActive={2} Where BankId={3}", bankObj.BankCode, bankObj.BankName, bankObj.IsActive, bankObj.BankId);
                    }
                }
                else
                {
                    return "Exist";
                }
             
                connection.ExecuteNonQuery(sql);
               
                rv = Operation.Success.ToString();
            }
            catch (Exception exception)
            {

                return exception.Message;
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        private int GetExistBank(string condition, CommonConnection connection)
        {
            string query = string.Format(" Select BankName From Bank {0}" , condition);
            DataTable dt = new DataTable();
            dt = connection.GetDataTable(query);
            var total = dt == null ? 0 : dt.Rows.Count;
            return total;
        }

        public string SaveBankBranch(BankBranch bankBranchObj)
        { 
            string rv = "";
            string sql = "";
            CommonConnection connection = new CommonConnection();
            try
            {
                string condition = "";
                if (bankBranchObj.BranchId == 0)
                {
                    condition = string.Format(" Where BankId ={2} And BranchName='{0}' or BranchCode={1}", bankBranchObj.BranchName, bankBranchObj.BranchCode,bankBranchObj.BankId);
                }
                else
                {
                    condition = string.Format(@" Where BankId ={3} And BranchId !={0} And (BranchName='{1}' or BranchCode={2})", bankBranchObj.BranchId, bankBranchObj.BranchName, bankBranchObj.BranchCode, bankBranchObj.BankId);
                }

                var res = GetExistBankBranch(condition, connection);

                if(res==0)
                {
                    if (bankBranchObj.BranchId == 0)
                    {
                        sql = string.Format(@"Insert Into BankBranch(BranchCode,BankId,BranchName,IsActive,Address) Values({0},{1},'{2}',{3},'{4}')", bankBranchObj.BranchCode, bankBranchObj.BankId, bankBranchObj.BranchName, bankBranchObj.IsActive,bankBranchObj.Address);
                    }
                    else
                    {
                        sql = string.Format(@"Update BankBranch Set BranchCode={0},BankId={1},BranchName='{2}',IsActive={3},Address='{4}' Where BranchId={5}", bankBranchObj.BranchCode, bankBranchObj.BankId, bankBranchObj.BranchName, bankBranchObj.IsActive,bankBranchObj.Address, bankBranchObj.BranchId);
                    }
                }
                else
                {
                    return "Exists";
                }
                connection.ExecuteNonQuery(sql);
                rv = Operation.Success.ToString();
            }
            catch (Exception exception)
            {

                return exception.Message;
            }
            finally
            {
                connection.Close();
            }
            return rv;
        }

        private int GetExistBankBranch(string condition, CommonConnection connection)
        {
            string query = string.Format(" Select BranchName From BankBranch {0}" , condition);
            DataTable dt = new DataTable();
            dt = connection.GetDataTable(query);
            var total = dt == null ? 0 : dt.Rows.Count;
            return total;
        }

        public List<Bank> GetAllBank()
        {
            string query = "Select * From Bank Where IsActive=1";

            var data = Kendo<Bank>.Combo.DataSource(query);
            return data;
        }

        public GridEntity<BankBranch> GetBankBranchSummary(GridOptions option)
        {
            string query = @"Select BankBranch.* ,BankName
            From BankBranch 
            left outer join Bank on BankBranch.BankId = Bank.BankId";

            var data = Kendo<BankBranch>.Grid.DataSource(option, query, "BranchName");
            return data;
        }

        public List<Bank> GetAllBankComboData()
        {
            string query = string.Format(@"Select * From Bank Where IsActive=1");
            var data = Kendo<Bank>.Combo.DataSource(query);
            return data;
        }

        public List<BankBranch> GetAllBranchByBankIdComboData(int bankId)
        {
            string query = string.Format(@"Select * From BankBranch Where BankId={0} And IsActive=1", bankId);
            var data = Kendo<BankBranch>.Combo.DataSource(query);
            return data;
        }
    }
}
