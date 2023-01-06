namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryConfigModel
    {
        public List<object> Credentials { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<EventNotificationConfig> EventNotificationConfigs { get; set; }
        public StateNotificationConfig StateNotificationConfig { get; set; }
        public HttpConfig HttpConfig { get; set; }
        public MqttConfig MqttConfig { get; set; }
        public string LogLevel { get; set; }

        public RegistryConfigModel()
        {
            Credentials = new List<object>();
            Id = string.Empty;
            Name = string.Empty;
            EventNotificationConfigs = new List<EventNotificationConfig>();
            MqttConfig = new MqttConfig();
            LogLevel = string.Empty;
            HttpConfig = new HttpConfig();
            StateNotificationConfig = new StateNotificationConfig();
        }
    }

    public class StateNotificationConfig
    {
        public string PubsubTopicName { get; set; }

        public StateNotificationConfig()
        {
            PubsubTopicName = string.Empty;
        }
    }

    public class EventNotificationConfig
    {
        public string PubsubTopicName { get; set; }

        public EventNotificationConfig()
        {
            PubsubTopicName = string.Empty;
        }
    }

    public class HttpConfig
    {
        public string HttpEnabledState { get; set; }

        public HttpConfig()
        {
            HttpEnabledState = string.Empty;
        }
    }

    public class MqttConfig
    {
        public string MqttEnabledState { get; set; }

        public MqttConfig()
        {
            MqttEnabledState = string.Empty;
        }
    }
}
