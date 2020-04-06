using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnitsReport.Models;
using UnitsReport.Services.Contracts;

namespace UnitsReport.Services
{
    public class ExcelService : IExcelService
    {
        /// <summary>
        /// Method to generate the byte stream for the given data
        /// </summary>
        /// <param name="data">Data to be streamed</param>
        /// <returns>Memory stream</returns>
        public async Task<Stream> GetDownloadStream(IEnumerable<UnitIntervalData> data)
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            return  stream;
        }
    }
}
