using RestSharp.Deserializers;

namespace Jira.Api.Models.Jira
{
    public class TimeSpent
    {
        [DeserializeAs(Name = "Timespent")]
        public int TimeSpentSeconds { get; set; }

        public decimal Hours { get; set; }
    }
}
