using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitsReport.Constants;

namespace UnitsReport.Models
{
    /// <summary>
    /// DB Repository implementation
    /// </summary>
    public class UnitIntervalsRepository : IIntervalDataRepository
    {
        private readonly AppDbContext _appDbContext;

        public UnitIntervalsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        /// <summary>
        /// Async method to retrieve date based report
        /// </summary>
        /// <param name="fromDate">From Date</param>
        /// <param name="toDate">To Date</param>
        /// <returns>List of interval data</returns>
        public async Task<IEnumerable<UnitIntervalData>> GetIntervalData(string fromDate,string toDate) {

            string query = string.Format(DBQuery.GetHourlyIntervalData, fromDate, toDate);
            return await _appDbContext.UnitIntervalData.FromSql(query).ToListAsync();
           
        }
    }
}
