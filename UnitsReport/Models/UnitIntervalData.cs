using System;

namespace UnitsReport.Models
{
    /// <summary>
    /// Class to represent Interval data unit.
    /// </summary>
    public class UnitIntervalData
    {
        public Int64 DeliveryPoint { get; set; }

        public DateTime Date { get; set; }

        public int TimeSlot { get; set; }

        public decimal Value { get; set; }
    }
}
