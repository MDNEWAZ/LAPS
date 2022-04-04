using System.Collections.Generic;
using Azolution.Entities.HumanResource;
using Azolution.HumanResource.DataService.DataService;
using Azolution.HumanResource.Service.Interface;
using Utilities;
using Utilities.Common.Json;

namespace Azolution.HumanResource.Service.Service
{
    public class BankBranchService : IBankBranchRepository
    {
        BankBranchDataService _dataService = new BankBranchDataService();

        public string SaveBank(Bank bankObj)
        {
            return _dataService.SaveBank(bankObj);
        }

        public string SaveBankBranch(BankBranch bankBranchObj)
        {
            return _dataService.SaveBankBranch(bankBranchObj);
        }

        public List<Bank> GetAllBank()
        {
            return _dataService.GetAllBank();
        }

        public string GetBankBranchSummary(GridOptions option)
        {
            JsonHelper jsonHelper= new JsonHelper();
            var data=_dataService.GetBankBranchSummary(option);
            return jsonHelper.GetJson(data);
        }

        public List<Bank> GetAllBankComboData()
        {
            return _dataService.GetAllBankComboData();
        }

        public List<BankBranch> GetAllBranchByBankIdComboData(int bankId)
        {
            return _dataService.GetAllBranchByBankIdComboData(bankId);
        }
    }
}
