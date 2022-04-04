using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Utilities;

namespace Laps.Sale.SaleDataService.DataService
{
   public class WaitingForDiscountDataService
    {
       public GridEntity<WaitingForDiscountSummaryDto> GetWaitingForDiscountSummary(GridOptions options, string companies)
        {
//            string sql = string.Format(@"Select SaleId,Invoice,Price,WarrantyStartDate,Sale.CustomerId,CustomerCode,SaleUserId,Sale.CompanyId,Sale.BranchId,IsSpecialDiscount,Name as CustomerName from Sale
//left outer join Sale_Customer on Sale_Customer.CustomerId = Sale.CustomerId
//where Sale.CompanyId in ({0}) and IsSpecialDiscount = 1 and Sale.IsActive = 0", companies);

            string sql = string.Format(@"Select DiscountId,Sale.SaleId,Invoice,Price,Sale.DownPay,WarrantyStartDate,Sale.CustomerId,CustomerCode,SaleUserId,SaleTypeId,
                    Sale.CompanyId,Sale.BranchId,IsSpecialDiscount,Name as CustomerName,Sale_Interest.Interests,
                    Sale.Installment As InstallmentNo,Sale.State,Sale.TempState,Sale_Product.DownPayPercent,Discount.DiscountOptionId,
                    Discount.DiscountTypeCode,Branch.BranchCode,SR.SalesRepId,DiscountType.DiscountTypeName,IsDPFixedAmount
                    From Discount
                    left outer join Sale on Sale.SaleId = Discount.SaleId
                    inner join Sale_Customer on Sale_Customer.CustomerId=Sale.CustomerId and Sale_Customer.IsActive=1
                    left outer join Sale_Interest on Sale_Interest.CompanyId = Sale_Customer.CompanyId And Sale_Interest.Status=1
                    left outer join Sale_Product on Sale_Product.ModelId=Sale.ModelId
                    Left outer join Branch on Branch.BranchId=Sale_Customer.BranchId
                    Left outer join SalesRepresentator SR on SR.SalesRepId= Sale.SalesRepId
                    Left outer join DiscountType on DiscountType.DiscountTypeCode=Discount.DiscountTypeCode
                    where Sale.CompanyId in ({0})and 
                    IsSpecialDiscount = 1 and Sale.IsActive = 0 And Discount.IsApprovedSpecialDiscount=0 and Sale.UnRecognizeType=1", companies);
            return Kendo<WaitingForDiscountSummaryDto>.Grid.DataSource(options,sql,"SaleId");
        }

        public string ApproveWaitingForDiscount(WaitingForDiscount objWaitingForDiscount, Users user)
        {
            CommonConnection connection = new CommonConnection();
            connection.BeginTransaction();
            string res = "";
            string updateQuery = "";
            StringBuilder qBuilder= new StringBuilder();
            try
            {
                if (objWaitingForDiscount.SaleTypeId == 1)
                {
                    qBuilder.Append(
                        string.Format(
                            @"Update Discount Set DiscountAmount={0},IsApprovedSpecialDiscount={1},ApprovedBy={2} Where DiscountId={3};",
                            objWaitingForDiscount.DiscountAmount, 1, user.UserId, objWaitingForDiscount.DiscountId));

                    qBuilder.Append(
                        string.Format(
                            @"Update Sale set Price={0},DownPay={1},NetPrice={2}, State={3},TempState={4} Where SaleId={5};",
                            objWaitingForDiscount.DiscountedAmount, objWaitingForDiscount.DownPay, objWaitingForDiscount.NetPrice,
                            2,objWaitingForDiscount.State,objWaitingForDiscount.SaleId));

                }
                else
                {
                    qBuilder.Append(
                        string.Format(
                            @"Update Discount Set DiscountAmount={0},IsApprovedSpecialDiscount={1},ApprovedBy={2} Where DiscountId={3};",
                            objWaitingForDiscount.DiscountAmount, 1, user.UserId, objWaitingForDiscount.DiscountId));

                    qBuilder.Append(
                        string.Format(
                            @"Update Sale set Price={0},NetPrice={1}, State={2},TempState={3} Where SaleId={4};",
                            objWaitingForDiscount.DiscountedAmount, objWaitingForDiscount.DiscountedAmount, 2,objWaitingForDiscount.State, objWaitingForDiscount.SaleId));
                }

                if (qBuilder.ToString() != "")
                {
                    updateQuery = "Begin " + qBuilder + " End;";
                    connection.ExecuteNonQuery(updateQuery);
                    res = Operation.Success.ToString();
                }
              
                
            }
            catch (Exception ex)
            {
                connection.RollBack();
                
                res = ex.Message;
            }finally
            {
                connection.CommitTransaction();
                connection.Close();
            }
            return res;
        }

        public Discount GetDiscountInfoByType(Users user)
        {
            if (user.ChangedCompanyId != 0)
            {
                user.CompanyId = user.ChangedCompanyId;
            }

            string sql = string.Format(@"Select DefaultCashDiscount,DefaultAgentDiscount,CashDiscountPercentage,AgentDiscountPercentage From Sale_Interest Where CompanyId={0} ", user.CompanyId);
            return Data<Discount>.DataSource(sql).SingleOrDefault();
        }

       public Discount GetDiscountInfo(int saleId)
       {
           string sql = string.Format(@"Select * From Discount Where SaleId={0}", saleId);
           var data= Data<Discount>.GenericDataSource(sql).SingleOrDefault();
           return data;
       }

       public List<DiscountType> GetDiscountTypeCombo()
       {
           string sql = string.Format(@"Select * From DiscountType Where IsActive=1");
           return Kendo<DiscountType>.Combo.DataSource(sql);
       }
    }
}
