using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnitsReport.Models;


namespace UnitsReport.Services.Contracts
{
    public interface IExcelService
    {
        /// <summary>
        /// Downloads excel   sheet
        /// </summary>
        /// <param name="FromDate">From Date</param>
        /// <param name="ToDate">To Date</param>
        /// <returns>File Stream</returns>
        Task<Stream> GetDownloadStream(IEnumerable<UnitIntervalData> data);
    }
}
