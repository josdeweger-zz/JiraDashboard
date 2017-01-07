using System.Collections.Generic;

namespace Jira.Api.Models.Response
{
    public class ValidationFailedResponse
    {
        public List<Error> Errors { get; set; }
        public List<FormattedError> FormattedErrors { get; set; }
        public bool IsValid { get; set; }
    }
}
