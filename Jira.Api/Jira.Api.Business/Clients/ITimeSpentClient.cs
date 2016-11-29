using System.Collections.Generic;
using Jira.Api.Models.Jira;

namespace Jira.Api.Business.Clients
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
