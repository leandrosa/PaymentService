using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Interfaces;

namespace Payments.Presentation.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        private INotificationContext _notificationContext;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected INotificationContext NotificationContext => _notificationContext ??= HttpContext.RequestServices.GetService<INotificationContext>();

        protected ActionResult CustomResponse(object obj = null)
        {
            if (NotificationContext.HasNotifications)
            {
                return BadRequest(GetValidationProblemDetails());
            }

            return Ok(obj);
        }

        protected ActionResult CustomResponse(string route, object obj = null)
        {
            if (NotificationContext.HasNotifications)
            {
                return BadRequest(GetValidationProblemDetails());
            }

            return CreatedAtRoute(route, obj, null);
        }

        private ValidationProblemDetails GetValidationProblemDetails()
        {
            var errors = NotificationContext.Notifications
                .GroupBy(e => e.Key, e => e.Message)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            var details = new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            return details;
        }
    }
}
