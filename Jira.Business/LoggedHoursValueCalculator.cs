using System;
using Jira.Models;
using Jira.Models.Config;

namespace Jira.Business
{
    public static class LoggedHoursValueCalculator
    {
        public static LoggedHoursValue CalculateLoggedHours(Sprint sprint, DateTime date, decimal hoursReserved, decimal hoursLogged, decimal faultMargin)
        {
            LoggedHoursValue loggedHoursValue;
            
            if (!sprint.IsDateInSprint(date))
            {
                loggedHoursValue = LoggedHoursValue.Neutral;
            }
            else if (hoursReserved <= 0)
            {
                loggedHoursValue = hoursLogged > 0 ? LoggedHoursValue.Negative : LoggedHoursValue.Positive;
            }
            else
            {
                var hoursExpected = CalculateHoursExpected(sprint, date, hoursReserved);
                var differenceHoursLoggedVersusHoursExpectedPercentage = 0m;
                if (hoursExpected > 0)
                    differenceHoursLoggedVersusHoursExpectedPercentage = Math.Abs(1 - hoursLogged / hoursExpected) * 100;

                loggedHoursValue = differenceHoursLoggedVersusHoursExpectedPercentage > faultMargin
                    ? LoggedHoursValue.Negative
                    : LoggedHoursValue.Positive;
            }

            return loggedHoursValue;
        }

        public static decimal CalculateHoursExpected(Sprint sprint, DateTime date, decimal hoursReserved)
        {
            var percentageOfDaysInSprint = (decimal) sprint.NrOfDayInSprint(date)/sprint.TotalNrOfDaysInSprint;

            return percentageOfDaysInSprint*hoursReserved;
        }
    }
}
