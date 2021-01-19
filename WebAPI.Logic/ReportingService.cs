using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataAccess;
using WebAPI.Models;

namespace WebAPI.Logic
{
    public class ReportingService: IReportingService
    {
        public IDataAccess _dataAccess;

        public ReportingService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public ReportingService()
        {
        }

        public async Task<WeeklySummary> GetWeeklySummary(int weekNumber)
        {
            DateTime firstDayOfWeek = GetFirstDayByWeekNumber(weekNumber);
            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);
            
            var result = await _dataAccess.GetTop10PlayersByScoreAndDuration(firstDayOfWeek, lastDayOfWeek);
            
            WeeklySummary summary = new WeeklySummary()
            {
                WeekNumber = weekNumber,
                TopPlayers = result.ToList()
            };
            return summary; //TODO: Convert seconds text
        }

        public Task<IList<PlayerImpactReportResult>> GetImpactReport()
        {
            var result = _dataAccess.GetImpactReport();
            return result;
        }

        // ISO 8601 
        public DateTime GetFirstDayByWeekNumber(int weekNumber) // NEEDS TEST
        {
            DateTime jan1 = new DateTime(2021, 1, 1);
            int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstMonday.AddDays(weekNum * 7 - 1);
            return result;
        }
    }
}