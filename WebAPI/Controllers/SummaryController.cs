using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DataAccess;
using WebAPI.Logic;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("reports")]
    public class SummaryController : Controller
    {
        public IReportingService _reportingService;

        public SummaryController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet]
        [Route("/summary/weekly/{weekNumber}")]
        public async Task<IActionResult> GetWeeklyReport(int weekNumber)
        {
            var result = await _reportingService.GetWeeklySummary(weekNumber);
            return Ok(result); //TODO: Convert seconds text to minutes
        }
        
        [HttpGet]
        [Route("impact")]
        public async Task<IActionResult> GetImpactReport()
        {
            var result = await _reportingService.GetImpactReport();
            return Ok(result);
        }
    }
}