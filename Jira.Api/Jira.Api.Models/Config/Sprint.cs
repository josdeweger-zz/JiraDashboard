using System;

namespace Jira.Api.Models.Config
{
    public class Sprint
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public int TotalNrOfDaysInSprint => (End - Start).Days + 1;

        public Sprint(DateTime start, DateTime end)
        {
            Start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            End = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
        }

        public bool IsDateInSprint(DateTime date)
        {
            return date >= Start && date <= End;
        }

        public int NrOfDayInSprint(DateTime date)
        {
            return IsDateInSprint(date) ? (date - Start).Days + 1 : 0;
        }
    }
}