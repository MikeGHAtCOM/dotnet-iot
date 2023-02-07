/*
 * Copyright (c) 2023 ClearBlade Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * Copyright (c) 2018 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */
 
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
