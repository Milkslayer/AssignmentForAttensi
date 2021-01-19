using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WebAPI.DataAccess;
using WebAPI.Logic;
using WebAPI.Models;

namespace Tests
{
    public class ReportingServiceTest
    {
        public static IEnumerable<object[]> GetFirstDayByWeekNumberData =>
            new List<object[]>
            {
                new object[] { 4, new DateTime(2021, 1, 25) },
                new object[] { 17, new DateTime(2021, 4, 26) },
                new object[] { 38, new DateTime(2021, 9, 20) },
                new object[] { 29, new DateTime(2021, 7, 19) },
                new object[] { 45, new DateTime(2021, 11, 8)},
                new object[] { 51, new DateTime(2021, 12, 20)},
            };
        
        [Theory]
        [MemberData(nameof(GetFirstDayByWeekNumberData))]
        public void GetFirstDayByWeekNumber(int weekNumber, DateTime expectedDate)
        {
            var reportingService = new ReportingService();

            var actualDate = reportingService.GetFirstDayByWeekNumber(weekNumber);
            
            Assert.Equal(0, DateTime.Compare(expectedDate,actualDate));
        }
    }
}