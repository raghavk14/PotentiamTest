using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnitsReport.Constants;
using UnitsReport.Models;
using UnitsReport.Services.Contracts;

namespace UnitsReport.Controllers
{
    /// <summary>
    /// Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// private field to hold the data repository
        /// </summary>
        private readonly IIntervalDataRepository _intervalRepository;
        
        /// <summary>
        /// private field to hold settings
        /// </summary>
        private readonly IOptions<AppSettings> _settings;

        /// <summary>
        /// field to hold logger instance
        /// </summary>
        private readonly ILogger _logger;

        private readonly IExcelService _excelService;
        
        public HomeController(IIntervalDataRepository intervalDataRepository,IOptions<AppSettings> appSettings, ILogger<HomeController> logger, IExcelService excelService)
        {
            _intervalRepository = intervalDataRepository;
            _logger = logger;
            _excelService = excelService;
            _settings = appSettings;  
        }

        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Action method to download the excel report
        /// </summary>
        /// <param name="fromDate">From Date & time</param>
        /// <param name="toDate">To Date & time</param>
        /// <returns>Excel file stream</returns>
        [HttpPost]  
        public async Task<IActionResult> Download(DateTime fromDate,DateTime toDate)
        {
            try
            {
                //TODO: Move this validation logic to a reusable method
                if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                {
                    ViewData[ApplicationConstants.ViewDataInvalidInputErrorKey] = ApplicationConstants.InvalidInputErrorString;

                    return View(ApplicationConstants.InvalidInputErrorView);
                }

                //Format the dates
                FormatDates(fromDate, toDate, out string formattedFromDate, out string formattedToDate);

                //Service composition
                var data = await _intervalRepository.GetIntervalData(formattedFromDate, formattedToDate);

                var stream = await _excelService.GetDownloadStream(data);

                string excelFileName = $"{_settings.Value.FileName}{DateTime.Now.ToString(_settings.Value.FileNameDateFormatString)}{_settings.Value.ExcelExtension}";

                return File(stream, $"{_settings.Value.ContentType}", excelFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Download), new string[] { fromDate.ToString(), toDate.ToString() });
            }

            ViewData[ApplicationConstants.ViewDataDownloadErrorKey] = ApplicationConstants.DownloadFailedErrorString ;

            return View(ApplicationConstants.ErrorView);

        }

        private void FormatDates(DateTime fromDate, DateTime toDate, out string formattedFromDate, out string formattedToDate)
        {
            formattedFromDate = fromDate.ToString(_settings.Value.DateFormatSetting);
            formattedToDate = toDate.ToString(_settings.Value.DateFormatSetting);
        }

        /// <summary>
        /// Action method Display Intervals data on the screen
        /// </summary>
        /// <param name="fromDate">From Date & time</param>
        /// <param name="toDate">To Date & time</param>
        /// <returns>Screen display</returns>
        [HttpPost]
        public async Task<IActionResult> Display(DateTime fromDate, DateTime toDate)
        {
            try
            {

                //TODO: Move this validation logic to a reusable method
                if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                {
                    ViewData[ApplicationConstants.ViewDataInvalidInputErrorKey] = ApplicationConstants.InvalidInputErrorString;

                    return View(ApplicationConstants.InvalidInputErrorView);
                }

                //Format the dates
                FormatDates(fromDate, toDate, out string formattedFromDate, out string formattedToDate);

                //Service composition
                var data = await _intervalRepository.GetIntervalData(formattedFromDate, formattedToDate);
                return PartialView(ApplicationConstants.IntervalsView, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Display));
            }

            ViewData[ApplicationConstants.ViewDataDownloadErrorKey] = ApplicationConstants.DisplayErrorString;

            return View(ApplicationConstants.ErrorView);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

   
}
