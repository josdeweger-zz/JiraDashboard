using FluentValidation;
using Jira.Models.Request;

namespace Jira.Api.Validators
{
    public class CustomerStatusRequestValidator : AbstractValidator<CustomerStatusRequest>
    {
        public CustomerStatusRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(request => request.TeamId).NotNull().NotEmpty().WithMessage("You must specify the team id.");
            RuleFor(request => request.ProjectKeys).NotNull().NotEmpty().WithMessage("You must specify at least one projectkey.");
            RuleFor(request => request.Date).NotNull().NotEmpty().WithMessage("You must specify a date.");
            RuleFor(request => request.Sprint).NotNull().NotEmpty().WithMessage("You must specify a sprint.");
            RuleFor(request => request.HoursReserved).NotNull().WithMessage("You must specify the hours reserved.");
        }
    }
}
