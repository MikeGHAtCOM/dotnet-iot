namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceCreateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int NumId { get; set; }
        public List<object> Credentials { get; set; }
        public LastErrorStatus LastErrorStatus { get; set; }
        public Config Config { get; set; }
        public State State { get; set; }
        public string LogLevel { get; set; }
        public Metadata Metadata { get; set; }
        public GatewayConfig GatewayConfig { get; set; }

        public DeviceCreateModel()
        {
            Id = String.Empty;
            Name = String.Empty;
            NumId = 0;
            Credentials = new List<object>();
            LastErrorStatus = new LastErrorStatus();
            Config = new Config();
            State = new State();
            LogLevel = String.Empty;
            Metadata = new Metadata();
            GatewayConfig = new GatewayConfig();
        }
    }
}
