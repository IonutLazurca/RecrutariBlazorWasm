using RecrutariBlazorWasm.Shared.Entities;
using System;
using System.Collections.Generic;

namespace RecrutariBlazorWasm.Shared.Extensions
{
    public static class WorkingDays
    {
        public static int CalculateWorkingDays(this Project project, List<DateTime> dates)
        {
            var start = DateTime.Now;
            var end = project.EndDate.Value;
            var totalDays = 0;
            for (var date = start.AddDays(1); date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !dates.Contains(date))
                {
                    totalDays++;
                }
            }
            return totalDays;

        }
    }
}
