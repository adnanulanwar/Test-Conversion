namespace DiDrDe.MessageBus.Infra.MassTransit.Configuration
{
    public class ActiveMqOptions
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EndpointName { get; set; }
        public bool UseSsl { get; set; }
        public bool AutoDelete { get; set; }
        //public RetryPolicyOptions RetryPolicyOptions { get; set; }
    }
}