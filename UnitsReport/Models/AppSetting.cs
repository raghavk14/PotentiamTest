namespace UnitsReport.Models
{
    /// <summary>
    /// Class to represent the application settings from appSettings.json
    /// </summary>
    public class AppSettings
    {
        public string DateFormatSetting { get; set; }

        public string FileNameDateFormatString { get; set; }

        public string FileName { get; set; }

        public string ExcelExtension { get; set; }

        public string ContentType { get; set; }
    }
}
