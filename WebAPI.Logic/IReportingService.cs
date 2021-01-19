using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public interface IReportingService
    {
        Task<WeeklySummary> GetWeeklySummary(int weekNumber);
        Task<IList<PlayerImpactReportResult>> GetImpactReport();
    }
}