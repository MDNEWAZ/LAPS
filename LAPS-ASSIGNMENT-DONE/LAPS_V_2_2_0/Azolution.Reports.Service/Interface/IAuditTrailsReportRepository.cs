using System.Collections.Generic;
using Azolution.Entities.Report;

namespace Azolution.Reports.Service.Interface
{
   public interface IAuditTrailsReportRepository
    {
       List<AuditTrailsReportEntity> GenerateAuditTrailsReport(ReportsParam<AuditTrailsReportEntity> objReportParams);
    }
}
