using System;
using System.Collections.Generic;

namespace Jira.Api.Models.Request
{
    public class CustomerStatusRequest
    {
        public int TeamId { get; set; }
        public List<string> ProjectKeys { get; set; }
        public DateTime? Date { get; set; }
        public Sprint Sprint { get; set; }
        public decimal? HoursReserved { get; set; }
    }
}
