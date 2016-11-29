using System;
using System.Collections.Generic;

namespace Jira.Api.Models.Jira
{
    public class Worklog
    {
        public int TimeSpentSeconds { get; set; }
        public DateTime DateStarted { get; set; }
        public string Comment { get; set; }
        public string Self { get; set; }
        public int Id { get; set; }
        public Author Author { get; set; }
        public Issue Issue { get; set; }
        public List<WorklogAttribute> WorklogAttributeses { get; set; }
    }
}
