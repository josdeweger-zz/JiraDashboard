using System;
using System.Collections.Generic;
using FluentAssertions;
using Jira.Business.Clients;
using Jira.Business.Stores;
using Jira.Models;
using Jira.Models.Config;
using Jira.Models.Jira;
using Moq;
using RestSharp;
using Xunit;

namespace Jira.Business.Specs
{
    public class LoggedHoursValueCalculatorSpecs
    {
        [Fact]
        public void WhenCurrentDateIsNotInSprintStatusValueShouldBeNeutral()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 11, 10);
            var hoursReserved = 8;
            var hoursLogged = 8;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Neutral);
        }

        [Fact]
        public void WhenHoursReservedIsZeroAndNoHoursAreLoggedTheStatusValueShouldBePositive()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 18);
            var hoursReserved = 0;
            var hoursLogged = 0;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Positive);
        }

        [Fact]
        public void WhenHoursReservedIsZeroAndSomeHoursAreLoggedTheStatusValueShouldBeNegative()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 18);
            var hoursReserved = 0;
            var hoursLogged = 8;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Negative);
        }

        /// <summary>
        /// nr of days in sprint = 13
        /// nr of days into sprint = 7
        /// percentage of days = 50%
        /// expected hours = 16
        /// hours logged = 28
        /// which is 12 more than expected
        /// which is 12 / 16 * 100 = 75%
        /// which is bigger than the faultmargin
        /// then LoggedHoursValue should be Negative
        /// </summary>
        [Fact]
        public void WhenHoursLoggedIsMoreThanHoursExpectedAndOutsideFaultMarginRangeTheStatusValueShouldBeNegative()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 20);
            var hoursReserved = 32;
            var hoursLogged = 28;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Negative);
        }

        /// <summary>
        /// nr of days in sprint = 13
        /// nr of days into sprint = 7
        /// percentage of days = 50%
        /// expected hours = 16
        /// hours logged = 4
        /// which is 12 less than expected
        /// which is 12 / 16 * 100 = 75%
        /// which is bigger than the faultmargin
        /// then LoggedHoursValue should be Negative
        /// </summary>
        [Fact]
        public void WhenHoursLoggedIsLessThanHoursExpectedAndOutsideFaultMarginRangeTheStatusValueShouldBeNegative()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 20);
            var hoursReserved = 32;
            var hoursLogged = 4;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Negative);
        }

        /// <summary>
        /// nr of days in sprint = 13
        /// nr of days into sprint = 7
        /// percentage of days = 50%
        /// expected hours = 16
        /// hours logged = 20
        /// which is 4 more than expected
        /// which is 4 / 16 * 100 = 25%
        /// which is less than the faultmargin
        /// then LoggedHoursValue should be Positive
        /// </summary>
        [Fact]
        public void WhenHoursLoggedIsMoreThanHoursExpectedButInsideFaultMarginRangeTheStatusValueShouldBePositive()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 20);
            var hoursReserved = 32;
            var hoursLogged = 20;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Positive);
        }

        /// <summary>
        /// nr of days in sprint = 13
        /// nr of days into sprint = 7
        /// percentage of days = 50%
        /// expected hours = 16
        /// hours logged = 12
        /// which is 4 less than expected
        /// which is 4 / 16 * 100 = 25%
        /// which is less than the faultmargin
        /// then LoggedHoursValue should be Positive
        /// </summary>
        [Fact]
        public void WhenHoursLoggedIsLessThanHoursExpectedButInsideFaultMarginRangeTheStatusValueShouldBePositive()
        {
            var sprint = new Sprint(new DateTime(2016, 10, 13), new DateTime(2016, 10, 26));
            var currentDate = new DateTime(2016, 10, 20);
            var hoursReserved = 32;
            var hoursLogged = 12;
            var faultMargin = 50;

            var status = LoggedHoursValueCalculator.CalculateLoggedHours(sprint, currentDate, hoursReserved, hoursLogged,
                faultMargin);

            status.Should().Be(LoggedHoursValue.Positive);
        }
    }
}
