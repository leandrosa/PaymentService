using System;
using FluentValidation;

namespace Payments.Domain.Common
{
    public abstract class Entity : Notifiable
    {
        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public void Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                AddNotifications(result);
            }
        }
    }
}
