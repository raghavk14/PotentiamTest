using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitsReport.Models
{
    public interface IIntervalDataRepository
    {
        Task<IEnumerable<UnitIntervalData>> GetIntervalData(string fromDate, string toDate);
    }
}