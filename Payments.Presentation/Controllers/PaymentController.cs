using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Common.Commands;
using Payments.Application.Common.DTOs;
using Payments.Application.Common.Queries;

namespace Payments.Presentation.Controllers
{
    [Authorize]
    [Route("payments")]
    public class PaymentController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public PaymentController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ProcessPayment([FromBody] PaymentCreation payment)
        {
            var command = _mapper.Map<ProcessPaymentCommand>(payment);
            var paymentId = await Mediator.Send(command);

            return CustomResponse("GetPayment", new { command.MerchantId, paymentId });
        }

        [HttpGet("{merchantId:int}", Name = "GetPayments")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(IEnumerable<PaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetPayments([FromRoute] int merchantId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetMerchantPaymentsQuery(merchantId, pageNumber, pageSize);

            var payments = await Mediator.Send(query);

            return CustomResponse(payments);
        }

        [HttpGet("{merchantId:int}/{paymentId:guid}", Name = "GetPayment")]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetPayment([FromRoute] int merchantId, [FromRoute] Guid paymentId)
        {
            var query = new GetMerchantPaymentByIdQuery(merchantId, paymentId);

            var payment = await Mediator.Send(query);

            return CustomResponse(payment);
        }
    }
}
