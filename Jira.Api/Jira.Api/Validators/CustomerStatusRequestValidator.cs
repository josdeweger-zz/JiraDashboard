using FluentValidation;
using Jira.Api.Models.Request;

namespace Jira.Api.Validators
{
    public class CustomerStatusRequestValidator : AbstractValidator<CustomerStatusRequest>
    {
        public CustomerStatusRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(r => r.TeamId).NotNull().NotEmpty();

            RuleFor(r => r.ProjectKeys).NotNull().NotEmpty();

            RuleFor(r => r.Date).NotNull().NotEmpty();

            RuleFor(r => r.Date)
                .GreaterThanOrEqualTo(r => r.Sprint.Start)
                .LessThanOrEqualTo(r => r.Sprint.End)
                .WithMessage("The selected date should be on or between sprint start and end date");
            
            RuleFor(r => r.Sprint.Start)
                .NotNull()
                .NotEmpty()
                .LessThan(r => r.Sprint.End)
                .WithMessage("The sprint start date has to be smaller than the sprint end date");

            RuleFor(r => r.HoursReserved).NotNull();
        }
    }
}
