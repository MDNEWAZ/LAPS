using System.Collections.Generic;
using Azolution.Entities.HumanResource;
using Utilities;

namespace Azolution.HumanResource.Service.Interface
{
    public interface IBankBranchRepository
    {
        string SaveBank(Bank bankObj);
        string SaveBankBranch(BankBranch bankBranchObj);
        List<Bank> GetAllBank();
        string GetBankBranchSummary(GridOptions option);
        List<Bank> GetAllBankComboData();
        List<BankBranch> GetAllBranchByBankIdComboData(int bankId);
    }
}
