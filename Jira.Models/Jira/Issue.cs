namespace Jira.Models.Jira
{
    public class Issue
    {
        public string Self { get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Key { get; set; }
        public int RemainingEstimateSeconds { get; set; }
        public IssueType IssueType { get; set; }
        public string Summary { get; set; }
    }
}
