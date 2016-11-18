namespace Jira.Models.Response
{
    public class CustomerStatusResponse
    {
        public decimal TotalHours { get; set; }
        public decimal HoursReserved { get; set; }
        public decimal HoursExpected { get; set; }
        public decimal Percentage { get; set; }
        public LoggedHoursValue LoggedHoursValue { get; set; }
    }
}
