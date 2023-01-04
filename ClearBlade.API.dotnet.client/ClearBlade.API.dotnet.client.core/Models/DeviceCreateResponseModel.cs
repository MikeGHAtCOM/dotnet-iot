namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceCreateResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NumId { get; set; }
        public List<object> Credentials { get; set; }
        public string LastHeartbeatTime { get; set; }
        public string LastEventTime { get; set; }
        public string LastStateTime { get; set; }
        public string LastConfigAckTime { get; set; }
        public string LastConfigSendTime { get; set; }
        public bool Blocked { get; set; }
        public string LastErrorTime { get; set; }
        public LastErrorStatus LastErrorStatus { get; set; }
        public Config Config { get; set; }
        public State State { get; set; }
        public string LogLevel { get; set; }
        public Metadata Metadata { get; set; }
        public GatewayConfig GatewayConfig { get; set; }

        public DeviceCreateResponseModel()
        {
            Id = String.Empty;
            Name = String.Empty;
            NumId = String.Empty;
            Credentials = new List<object>();
            LastHeartbeatTime = String.Empty;
            LastEventTime = String.Empty;
            LastStateTime = String.Empty;
            LastConfigAckTime = String.Empty;
            LastConfigSendTime = String.Empty;
            LastErrorTime = String.Empty;
            LastErrorStatus = new LastErrorStatus();
            Config = new Config();
            State = new State();
            LogLevel = String.Empty;
            Metadata = new Metadata();
            GatewayConfig = new GatewayConfig();
        }
    }
}
