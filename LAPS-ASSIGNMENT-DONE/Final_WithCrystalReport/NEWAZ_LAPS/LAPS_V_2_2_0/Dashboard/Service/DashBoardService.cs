using System;
using System.Collections.Generic;
using Azolution.Dashboard.DataService;
using Azolution.Dashboard.Service.Interface;
using Azolution.Entities.Core;
using Azolution.Entities.Sale;
using Utilities;

namespace Azolution.Dashboard.Service.Service
{
    public class DashBoardService : IDashboardRepository
    {
        DashboardDataService _dashboardDataService = new DashboardDataService();
        public List<PendingCollectionChart> GetAllPendingCollectionsForDashBoard(Users objUser, int companyId, int branchId, bool isAdministrator, DateTime? fromDate, DateTime? toDate, string companies)
        {

            //here PendingCollections means month wise bussiness trend graph's data
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";
            if (isAdministrator != true)
            {
               
                whereCondition = "where CompanyId in (" + companies + ") ";
            }
            else
            {


                if (isAdministrator == true && (companyId != 0 || branchId != 0))
                {
                    if (companyId != 0)
                    {
                        topParam = "CompanyId,";
                        groupParam = ",tblAll.CompanyId";
                        whereCondition = "where CompanyId=" + companyId;
                    }
                    if (branchId != 0)
                    {
                        topParam = "BranchId,";
                        groupParam = ",tblAll.BranchId";
                        whereCondition = "where BranchId=" + branchId;
                    }
                    if (companyId != 0 && branchId != 0)
                    {
                        topParam = "CompanyId,BranchId,";
                        groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                        whereCondition = "where CompanyId=" + companyId + " and BranchId = " + branchId;
                    }
                }
                if (isAdministrator == true && (companyId == 0 && companies != ""))
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";

                    whereCondition = "where CompanyId in (" + companies + ")";

                }
            }


            return _dashboardDataService.GetAllPendingCollectionsForDashBoard(objUser, topParam, groupParam, whereCondition, fromDate, toDate);
        }

        public List<PendingCollectionChart> GetMonthWiseCollectionData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate,string companies)
        {
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";

            if (companyId != 0 || branchId != 0)
            {
                if (companyId != 0)
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";
                    whereCondition = "and CompanyId=" + companyId;
                }
                if (branchId != 0)
                {
                    topParam = "BranchId,";
                    groupParam = ",tblAll.BranchId";
                    whereCondition = "and BranchId=" + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    topParam = "CompanyId,BranchId,";
                    groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                    whereCondition = "and CompanyId=" + companyId + " and BranchId = " + branchId;
                }
            }
            else
            {
                topParam = "CompanyId,";
                groupParam = ",tblAll.CompanyId";
                whereCondition = "and CompanyId in (" + companies + " )";
            }

            return _dashboardDataService.GetMonthWiseCollectionData(userId, topParam, groupParam, whereCondition, fromDate, toDate);
        }

        public List<PendingCollectionChart> GetMonthWiseSalesData(int userId, int companyId, int branchId, DateTime? fromDate, DateTime? toDate,string companies)
        {
            string topParam = "";

            string groupParam = "";

            string whereCondition = "";

            if (companyId != 0 || branchId != 0)
            {
                if (companyId != 0)
                {
                    topParam = "CompanyId,";
                    groupParam = ",tblAll.CompanyId";
                    whereCondition = "and  CompanyId=" + companyId;
                }
                if (branchId != 0)
                {
                    topParam = "BranchId,";
                    groupParam = ",tblAll.BranchId";
                    whereCondition = "and BranchId=" + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    topParam = "CompanyId,BranchId,";
                    groupParam = ",tblAll.CompanyId,tblAll.BranchId";
                    whereCondition = "and CompanyId=" + companyId + " and BranchId = " + branchId;
                }
            }
            else
            {
                topParam = "CompanyId,";
                groupParam = ",tblAll.CompanyId";
                whereCondition = "and CompanyId in (" + companies + " )";
            }

            return _dashboardDataService.GetMonthWiseSalesData(userId, topParam, groupParam, whereCondition, fromDate, toDate);
        }

        public GridEntity<RatingCalculation> GetDueCollectionCustomerGridData(GridOptions options, bool isAdministrator, Users objUser, int companyId, int branchId, string companies)
        {
            string condition = "";
            if (isAdministrator != true)
            {

                condition = "Where sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId;
            }

            else if (isAdministrator == true && (companyId != 0 || branchId != 0))
            {
                if (companyId != 0)
                {
                    condition = " where sc.CompanyId =  " + companyId;
                }
                if (branchId != 0)
                {
                    condition = " where sc.BranchId =  " + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    condition = " where sc.CompanyId =" + companyId + " and sc.BranchId =  " + branchId;
                }
            }

            if (isAdministrator == true && (companyId == 0 && companies != ""))
            {
                condition += condition == ""
                             ? "Where sc.CompanyId in ( " + companies + " ) "
                             : " And sc.CompanyId in ( " + companies + " ) ";
            }
            string orderBy = "TotalDuePercentTillDate";
            string condition2 = "where tblTemp3.TotalDuePercentTillDate != 0";
            return _dashboardDataService.GetDuePercentAndCustomerInfoForDashboardGrid(options, condition, condition2, orderBy);
        }

        public object GetCusomerRatingInfoData(GridOptions options, string invoice, bool isAdministrator, Users objUser, int companyId,int branchId, string companies)
        {

            string condition = "";
            string companyAndbranchId = "";
            if (invoice != "0")
            {
                condition += condition == "" ? " SI.SInvoice=" + invoice : " and SI.SInvoice=" + invoice;
            }


            if (isAdministrator != true)
            {
                condition += condition == ""
                                ? " sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId
                                : " And sc.CompanyId = " + objUser.CompanyId + " and sc.BranchId = " + objUser.BranchId;
            }

            else if (isAdministrator == true && (companyId != 0 || branchId != 0))
            {
                if (companyId != 0)
                {
                    companyAndbranchId = "  sc.CompanyId =  " + companyId;
                }
                if (branchId != 0)
                {
                    companyAndbranchId = "  sc.BranchId =  " + branchId;
                }
                if (companyId != 0 && branchId != 0)
                {
                    companyAndbranchId = "  sc.CompanyId =" + companyId + " and sc.BranchId =  " + branchId;
                }
                condition += condition == "" ? companyAndbranchId : " and " + companyAndbranchId;
            }
            if (isAdministrator == true && (companyId == 0 && companies != ""))
            {
                condition += condition == ""
                             ? " sc.CompanyId in ( " + companies + " ) "
                             : " And sc.CompanyId in ( " + companies + " ) ";
            }
            if (condition != "")
            {
                condition = "Where " + condition;
            }
            string orderBy = "TotalDuePercentTillDate desc";
            string condition2 = "";
            return _dashboardDataService.GetCusomerRatingInfoData(options, condition, condition2, orderBy);
        }
    }
}
