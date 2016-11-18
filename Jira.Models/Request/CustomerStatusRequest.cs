using System;
using System.Collections.Generic;

namespace Jira.Models.Request
{
    public class CustomerStatusRequest
    {
        public List<string> ProjectKeys { get; set; }
        public DateTime Date { get; set; }
        public Sprint Sprint { get; set; }
        public decimal HoursReserved { get; set; }
    }
}
