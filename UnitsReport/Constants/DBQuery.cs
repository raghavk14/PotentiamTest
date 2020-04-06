namespace UnitsReport.Constants
{
    public static class DBQuery
    {
        public static  string GetHourlyIntervalData = @"SELECT DeliveryPoint,[Date],CAST(DENSE_RANK() OVER (ORDER BY DATEPART(HH,TimeSlot) ASC) - 1 AS INTEGER)  AS TimeSlot,
                                                                SUM(SlotVal) AS Value FROM dbo.IntervalData (NOLOCK)
                                                                WHERE [DATE] between '{0}' AND '{1}'                                                           
                                                                GROUP BY DeliveryPoint,[Date],DATEPART(HH,TimeSlot)
                                                                ORDER BY [Date],TimeSlot ASC";
    }
}
