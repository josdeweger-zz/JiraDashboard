using System.Collections.Generic;
using Jira.Models.Jira;

namespace Jira.Business.Clients
{
    public interface ITimeSpentClient
    {
        ITimeSpentClient WithBaseUrl(string baseUrl);
        ITimeSpentClient FilterProject(string projectKey);
        ITimeSpentClient FilterProjects(string[] projectKeys);
        ITimeSpentClient FilterIssueType(IssueTypeEnum issueType);
        ITimeSpentClient FilterIssueTypes(IssueTypeEnum[] issueTypes);
        List<TimeSpent> Get();
    }
}
