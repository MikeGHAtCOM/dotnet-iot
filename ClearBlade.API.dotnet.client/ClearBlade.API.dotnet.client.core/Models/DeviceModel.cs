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
    public class Config
    {
        public DateTime CloudUpdateTime { get; set; }
        public string Version { get; set; }

        public Config()
        {
            Version = String.Empty;
        }
    }

    public class DeviceCollection
    {
        public List<DeviceModel> Devices { get; set; }

        public DeviceCollection()
        {
            Devices = new List<DeviceModel>();
        }
    }

    public class DeviceCredential
    {
        public object? ExpirationTime { get; set; }
        public PublicKeyCredential PublicKey { get; set; }
        public string ETag { get; set; }

        public DeviceCredential()
        {
            PublicKey = new PublicKeyCredential();
            ETag = string.Empty;
        }
        public DeviceCredential(object expirationTime, PublicKeyCredential publicKey, string eTag)
        {
            ExpirationTime = expirationTime;
            PublicKey = publicKey;
            ETag = eTag;
        }
    }

    public class PublicKeyCredential
    {
        public string Format { get; set; }
        public string Key { get; set; }
        public string ETag { get; set; }

        public PublicKeyCredential()
        {
            Format = String.Empty;
            Key = String.Empty;
            ETag = String.Empty;
        }
        public PublicKeyCredential(string format, string key, string eTag)
        {
            Format = format;
            Key = key;
            ETag = eTag;
        }
    }

    /// <summary>
    /// Class that represents a device
    /// </summary>
    public class DeviceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NumId { get; set; }
        public List<DeviceCredential> Credentials { get; set; }
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

        public DeviceModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            NumId = string.Empty;
            Credentials = new List<DeviceCredential>();
            LastHeartbeatTime = string.Empty;
            LastEventTime = string.Empty;
            LastStateTime = string.Empty;
            LastConfigAckTime = string.Empty;
            LastConfigSendTime = string.Empty;
            LastErrorTime = string.Empty;
            LastErrorStatus = new LastErrorStatus();
            Config = new Config();
            State = new State();
            LogLevel = "NONE";
            Metadata = new Metadata();
            GatewayConfig = new GatewayConfig();
        }

        public DeviceModel(DeviceCreateResponseModel? item2)
        {
            if (item2 != null)
            {
                Id = item2.Id;
                Name = item2.Name;
                NumId = item2.NumId;
                Credentials = item2.Credentials;
                LastHeartbeatTime = item2.LastHeartbeatTime;
                LastEventTime = item2.LastEventTime;
                LastStateTime = item2.LastStateTime;
                LastConfigAckTime = item2.LastConfigAckTime;
                LastConfigSendTime = item2.LastConfigSendTime;
                LastErrorTime = item2.LastErrorTime;
                LastErrorStatus = item2.LastErrorStatus;
                Config = item2.Config;
                State = item2.State;
                LogLevel = item2.LogLevel;
                Metadata = item2.Metadata;
                GatewayConfig = item2.GatewayConfig;
            }
            else
            {
                Id = string.Empty;
                Name = string.Empty;
                NumId = string.Empty;
                Credentials = new List<DeviceCredential>();
                LastHeartbeatTime = string.Empty;
                LastEventTime = string.Empty;
                LastStateTime = string.Empty;
                LastConfigAckTime = string.Empty;
                LastConfigSendTime = string.Empty;
                LastErrorTime = string.Empty;
                LastErrorStatus = new LastErrorStatus();
                Config = new Config();
                State = new State();
                LogLevel = "NONE";
                Metadata = new Metadata();
                GatewayConfig = new GatewayConfig();
            }
        }
    }

    public class GatewayConfig
    {
        public string LastAccessedGatewayId { get; set; }
        public string LastAccessedGatewayTime { get; set; }
        public string GatewayType { get; set; }
        public string GatewayAuthMethod { get; set; }

        public GatewayConfig()
        {
            LastAccessedGatewayId = string.Empty;
            LastAccessedGatewayTime = string.Empty;
            GatewayType = string.Empty;
            GatewayAuthMethod = string.Empty;
        }
    }

    public class LastErrorStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public int code { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public string message { get; set; }

        public LastErrorStatus()
        {
            message = string.Empty;
        }
    }

#pragma warning disable S2094 // Classes should not be empty
    public class Metadata : Dictionary<string, string>
#pragma warning restore S2094 // Classes should not be empty
    {
    }

    public class PublicKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public string format { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public string key { get; set; }

        public PublicKey()
        {
            format = string.Empty;
            key = string.Empty;
        }
    }

    public class State
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public string updateTime { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to match JSON")]
        public string binaryData { get; set; }

        public State()
        {
            updateTime = String.Empty;
            binaryData = String.Empty;
        }
    }


}
