using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core.Services
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly string _accessToken;
        /// <summary>
        /// Constructor which initializes the access token for sending any request to devices webhook
        /// </summary>
        /// <param name="accessToken"></param>
        public AuthHeaderHandler(string accessToken)
        {
            _accessToken = accessToken;
            InnerHandler = new HttpClientHandler();
        }
        /// <summary>
        /// Method to send the request asynchronously
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response message</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // assuming that the input is token
            request.Headers.Add("ClearBlade-UserToken", _accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
