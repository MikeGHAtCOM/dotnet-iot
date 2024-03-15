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
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly string _accessToken;

        public HttpLoggingHandler(string accessToken, HttpMessageHandler? innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler())
        {
            _accessToken = accessToken;
        }
        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = request;
            var id = Guid.NewGuid().ToString();
            var msg = $"[{id} -   Request]";

            Debug.WriteLine($"{msg}========Start==========");
            Debug.WriteLine($"{msg} {req.Method} {req?.RequestUri?.PathAndQuery} {req?.RequestUri?.Scheme}/{req?.Version}");
            Debug.WriteLine($"{msg} Host: {req?.RequestUri?.Scheme}://{req?.RequestUri?.Host}");

#pragma warning disable S2589 // Boolean expressions should not be gratuitous
            if (req != null)
#pragma warning restore S2589 // Boolean expressions should not be gratuitous

            {
                foreach (var header in req.Headers)
                    Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

                if (req.Content != null)
                {
                    foreach (var header in req.Content.Headers)
                        Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

                    if (req.Content is StringContent || this.IsTextBasedContentType(req.Headers) || this.IsTextBasedContentType(req.Content.Headers))
                    {
#if NET48
                        var result = await req.Content.ReadAsStringAsync();
#else
                        var result = await req.Content.ReadAsStringAsync(cancellationToken);
#endif

                        Debug.WriteLine($"{msg} Content:");
                        Debug.WriteLine($"{msg} {string.Join("", result.Cast<char>().Take(255))}...");

                    }
                }
            }

            var start = DateTime.Now;

            // assuming that the input is token
            request.Headers.Add("ClearBlade-UserToken", _accessToken);

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var end = DateTime.Now;

            Debug.WriteLine($"{msg} Duration: {end - start}");
            Debug.WriteLine($"{msg}==========End==========");

            msg = $"[{id} - Response]";
            Debug.WriteLine($"{msg}=========Start=========");

            var resp = response;

            Debug.WriteLine($"{msg} {req?.RequestUri?.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

            foreach (var header in resp.Headers)
                Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (resp.Content != null)
            {
                foreach (var header in resp.Content.Headers)
                    Debug.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

                if (resp.Content is StringContent || this.IsTextBasedContentType(resp.Headers) || this.IsTextBasedContentType(resp.Content.Headers))
                {
                    start = DateTime.Now;
#if NET48
                    var result = await req.Content.ReadAsStringAsync();
#else
                        var result = await req.Content.ReadAsStringAsync(cancellationToken);
#endif
                    end = DateTime.Now;

                    Debug.WriteLine($"{msg} Content:");
                    Debug.WriteLine($"{msg} {string.Join("", result.Cast<char>().Take(8192))}...");
                    Debug.WriteLine($"{msg} Duration: {end - start}");
                }
            }

            Debug.WriteLine($"{msg}==========End==========");
            return response;
        }

        readonly string[] types = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        bool IsTextBasedContentType(HttpHeaders headers)
        {
            if (!headers.TryGetValues("Content-Type", out IEnumerable<string>? values) || values == null || !values.Any())
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}
