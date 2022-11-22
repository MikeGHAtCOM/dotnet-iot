using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class RegistryConfigModel
    {
        public List<object> credentials { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public List<EventNotificationConfig> eventNotificationConfigs { get; set; }
        public StateNotificationConfig stateNotificationConfig { get; set; }
        public HttpConfig httpConfig { get; set; }
        public MqttConfig mqttConfig { get; set; }
        public string logLevel { get; set; }

        public RegistryConfigModel()
        {
            credentials = new List<object>();
            id = string.Empty;
            name = string.Empty;
            eventNotificationConfigs = new List<EventNotificationConfig>();
            mqttConfig = new MqttConfig();
            logLevel = string.Empty;
            httpConfig = new HttpConfig();
            stateNotificationConfig = new StateNotificationConfig();
        }
    }

    public class StateNotificationConfig
    {
        public string pubsubTopicName { get; set; }

        public StateNotificationConfig()
        {
            pubsubTopicName = string.Empty;
        }
    }

    public class EventNotificationConfig
    {
        public string pubsubTopicName { get; set; }

        public EventNotificationConfig()
        {
            pubsubTopicName = string.Empty;
        }
    }

    public class HttpConfig
    {
        public string httpEnabledState { get; set; }

        public HttpConfig()
        {
            httpEnabledState = string.Empty;
        }
    }

    public class MqttConfig
    {
        public string mqttEnabledState { get; set; }

        public MqttConfig()
        {
            mqttEnabledState = string.Empty;
        }
    }
}
