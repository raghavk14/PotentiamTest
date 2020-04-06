using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnitsReport.Controllers;
using UnitsReport.Models;
using UnitsReport.Services.Contracts;
using Xunit;

namespace XUnitTests
{
    public class HomeControllerTest
    {
        private ILoggerFactory _loggerFactory;
        private ILogger<HomeController> _controllerlogger;

        public HomeControllerTest()
        {
            createLogger();
        }

        private void createLogger()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _loggerFactory = factory;
            _controllerlogger = factory.CreateLogger<HomeController>();
        }
        private IIntervalDataRepository getIntervalsRepository()
        {
            Mock<IIntervalDataRepository> mockIntervalRepo = new Mock<IIntervalDataRepository>();
            mockIntervalRepo.Setup(repo => repo.GetIntervalData(It.IsAny<string>(), It.IsAny<string>())).Returns(GetIntervalData);
            return mockIntervalRepo.Object;
        }

        private async Task<IEnumerable<UnitIntervalData>> GetIntervalData() => new List<UnitIntervalData>
        {
            new UnitIntervalData
            {
                Date = System.DateTime.Now,
                DeliveryPoint = 123456,
                TimeSlot = 1,
                Value = 2
            }
        };
        private IExcelService getExcelServiceInstance()
        {
            Mock<IExcelService> mockExcelService = new Mock<IExcelService>();
            mockExcelService.Setup(svc => svc.GetDownloadStream(GetIntervalData().Result)).Returns(GetMemoryStream);
            return mockExcelService.Object;
        }

        private async Task<Stream> GetMemoryStream() => new MemoryStream(Encoding.UTF8.GetBytes("test"));

        private static IOptions<AppSettings> getServiceSettings()
        {
            return Options.Create(new AppSettings()
            {
                DateFormatSetting = "yyyy-MM-dd HH:mm:ss.fff",
                FileNameDateFormatString = "yyyyMMddHHmmssfff",
                FileName = "TestBusinessUnits-",
                ExcelExtension = ".xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });
        }
        public IOptions<AppSettings> SystemSettings => getServiceSettings();
       
        public IIntervalDataRepository intervalsRepository => getIntervalsRepository();

        public IExcelService excelService => getExcelServiceInstance();
       
        [Fact]
        public void ControllerTest()
        {
            //Arrange
            HomeController homeController = new HomeController(intervalsRepository, SystemSettings, _controllerlogger, excelService);
            //Assert
            Assert.NotNull(homeController);
            Assert.IsAssignableFrom<HomeController>(homeController);
        }

        [Fact]
        public async void DownloadExcelTest()
        {
            //Arrange
            HomeController homeController = new HomeController(intervalsRepository, SystemSettings, _controllerlogger, excelService);
            homeController.ControllerContext = new ControllerContext();
            homeController.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var result = await homeController.Download(new System.DateTime(2018, 01, 01, 12, 0, 0, 0), new System.DateTime(2018, 01, 02, 12, 0, 0, 0));

            //Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}


