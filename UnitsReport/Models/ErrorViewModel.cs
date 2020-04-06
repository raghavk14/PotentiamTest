using System;

namespace UnitsReport.Models
{
    //View model to track request errors
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}