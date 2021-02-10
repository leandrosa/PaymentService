namespace Payments.Application.Common.Configurations
{
    public class IssuerConfiguration
    {
        public bool Mocked { get; set; }

        public int TimeoutInSeconds { get; set; }

        public string Uri { get; set; }

        public string UserAgent { get; set; }
    }
}
