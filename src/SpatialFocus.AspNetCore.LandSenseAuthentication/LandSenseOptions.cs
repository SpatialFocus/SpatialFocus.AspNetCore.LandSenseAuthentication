// <copyright file="LandSenseOptions.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication
{
	using Microsoft.AspNetCore.Authentication;

	public class LandSenseOptions : AuthenticationSchemeOptions
	{
		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string UserInfoEndpoint { get; set; }
	}
}