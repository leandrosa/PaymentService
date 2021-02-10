using System;

namespace Payments.Application.Common.DTOs
{
    public class IssuerResponse
    {
        public Guid ProcessingId { get; set; }

        public int ProcessingStatus { get; set; }
    }
}
