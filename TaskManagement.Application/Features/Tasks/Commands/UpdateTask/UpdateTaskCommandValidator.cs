using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان نباید خالی باشد.")
                .MaximumLength(100).WithMessage("عنوان نمی‌تواند بیش از 100 کاراکتر باشد.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.DueDate.HasValue)
                .WithMessage("تاریخ سررسید نمی‌تواند قبل از امروز باشد.");
        }
    }
}
