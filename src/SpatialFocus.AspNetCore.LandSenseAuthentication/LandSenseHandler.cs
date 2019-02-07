// <copyright file="LandSenseHandler.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Text.Encodings.Web;
	using System.Threading.Tasks;
	using Flurl.Http;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using Newtonsoft.Json.Linq;

	public class LandSenseHandler : AuthenticationHandler<LandSenseOptions>
	{
		public LandSenseHandler(IOptionsMonitor<LandSenseOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
			: base(options, logger, encoder, clock)
		{
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
			{
				return AuthenticateResult.NoResult();
			}

			string token = null;

			string authorization = Request.Headers["Authorization"];

			// If no authorization header found, nothing to process further
			if (string.IsNullOrEmpty(authorization))
			{
				return AuthenticateResult.NoResult();
			}

			if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			{
				token = authorization.Substring("Bearer ".Length).Trim();
			}

			// If no token found, no further work possible
			if (string.IsNullOrEmpty(token))
			{
				return AuthenticateResult.NoResult();
			}

			try
			{
				string result = await Options.UserInfoEndpoint.WithOAuthBearerToken(token)
					.PostUrlEncodedAsync(new
					{
						client_id = Options.ClientId,
						client_secret = Options.ClientSecret,
					})
					.ReceiveString();

				JObject user = JObject.Parse(result);

				ICollection<Claim> claims = new List<Claim>();

				foreach (JProperty property in user.Properties().Select(x => x))
				{
					if (!property.Value.Any())
					{
						claims.Add(new Claim(property.Name, (string)property.Value));
					}
					else
					{
						foreach (JToken jToken in property.Value)
						{
							claims.Add(new Claim(property.Name, (string)jToken));
						}
					}
				}

				ClaimsIdentity identity = new ClaimsIdentity(claims, "AuthenticationTypes.Federation");

				ClaimsPrincipal principal = new ClaimsPrincipal(identity);
				AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

				return AuthenticateResult.Success(ticket);
			}
			catch (FlurlHttpException e)
			{
				return AuthenticateResult.Fail(await e.GetResponseStringAsync());
			}
		}
	}
}