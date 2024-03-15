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

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ClearBlade.API.dotnet.client.core.Models
{
    public class DeviceCreateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public int? NumId { get; set; }
        public List<DeviceCredential> Credentials { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("LastErrorStatus", NullValueHandling = NullValueHandling.Ignore)]
        public LastErrorStatus? LastErrorStatus { get; set; }
        public Config Config { get; set; }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        [JsonProperty("State", NullValueHandling = NullValueHandling.Ignore)]
        public State? State { get; set; }

        public string LogLevel { get; set; }
        public Metadata Metadata { get; set; }
        public GatewayConfig GatewayConfig { get; set; }

        public DeviceCreateModel()
        {
            Id = String.Empty;
            Name = String.Empty;
            NumId = null;
            Credentials = new List<DeviceCredential>();
            LastErrorStatus = new LastErrorStatus();
            Config = new Config();
            State = new State();
            LogLevel = String.Empty;
            Metadata = new Metadata();
            GatewayConfig = new GatewayConfig();
        }
    }
}
