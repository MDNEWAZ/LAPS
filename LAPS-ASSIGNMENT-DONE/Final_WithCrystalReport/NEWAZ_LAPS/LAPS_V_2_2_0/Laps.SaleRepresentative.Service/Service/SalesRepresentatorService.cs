using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Azolution.Entities.Core;
using Azolution.Entities.DTO;
using Azolution.Entities.Sale;
using Laps.AdminSettings.Service.Service;
using Laps.SaleRepresentative.DataService.DataService;
using Laps.SaleRepresentative.Service.Interface;
using Utilities;
using Utilities.Common.Json;

namespace Laps.SaleRepresentative.Service.Service
{
    public class SalesRepresentatorService:ISalesRepresentatorRepository
    {
        SalesRepresentatorDataService _representatorDataService = new SalesRepresentatorDataService();
        JsonHelper jsonHelper = new JsonHelper();

        public string SaveSalesRepresentator(SalesRepresentator objSalesRepresentator)
        {
            return _representatorDataService.SaveSalesRepresentator(objSalesRepresentator);
        }

        public string GetAllSalesRepresentator(GridOptions options)
        {
            var data= jsonHelper.GetJson(_representatorDataService.GetAllSalesRepresentator(options));
            CommissionAndIncentiveCalculation _calculation = new CommissionAndIncentiveCalculation();
          //  _calculation.CommissionIncentiveCalculation();
            return data;
        }

        public object GetSalesRepresentatorType()
        {
            return _representatorDataService.GetSalesRepresentatorType();
        }

        public List<SalesRepresentator> GetAllSalesRepresentatorCombo(int salesRepType, Users user)
        {
            string condition = "";

            if (salesRepType > 0)
            {
                condition = " And SalesRepType=" + salesRepType;
            }
            if (user.ChangedCompanyId == 0)
            {
                condition += " And CompanyId=" + user.CompanyId;
            }
            else
            {
                condition += " And CompanyId=" + user.ChangedCompanyId;
            }

            if (user.ChangedBranchId == 0)
            {
                condition += " And BranchId=" + user.BranchId;
            }
            else
            {
                condition += " And BranchId=" + user.ChangedBranchId;
            }
            return _representatorDataService.GetAllSalesRepresentatorCombo(condition);
        }
        public object GetAllSalesRepresentatorCombo()
        {
            return _representatorDataService.GetAllSalesRepresentatorCombo();
        }
        public object GetAllSalesRepresentatorById(string salesRepId)
        {
            return _representatorDataService.GetAllSalesRepresentatorById(salesRepId);
        }

        public List<SalesRepresentator> GetSalesRepresentatorByCompanyAndBranch(int companyId, int branchId, Users user)
        {
            string condition = string.Empty;
            if (companyId > 0)
            {
                condition = " And CompanyId=" + companyId;
            }
            if (branchId > 0)
            {
                condition += " And BranchId=" + branchId;
            }
            if (companyId == 0 && branchId == 0)
            {
                condition = " And CompanyId=" + user.CompanyId + " And BranchId=" + user.BranchId;
            }
            return _representatorDataService.GetSalesRepresentatorByCompanyAndBranch(condition);
        }

        public List<SalesRepresentator> GetSalesRepComboByCompanyBranchAndType(int salesRepType, int companyId, int branchId, Users user)
        {
            string condition = "";

            if (salesRepType > 0)
            {
                condition = " And SalesRepType=" + salesRepType;
            }
            if (companyId==0)
            {
                condition += " And CompanyId=" + user.CompanyId;
            }
            else
            {
                condition += " And CompanyId=" + companyId;
            }

            if (branchId == 0)
            {
                condition += " And BranchId=" + user.BranchId;
            }
            else
            {
                condition += " And BranchId=" + branchId;
            }
            return _representatorDataService.GetSalesRepComboByCompanyBranchAndType(condition);

        }
    }
}
