// <copyright file="LandSensePostConfigureOptions.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication
{
	using System;
	using Microsoft.Extensions.Options;

	public class LandSensePostConfigureOptions : IPostConfigureOptions<LandSenseOptions>
	{
		public void PostConfigure(string name, LandSenseOptions options)
		{
			if (string.IsNullOrEmpty(options.ClientId))
			{
				throw new InvalidOperationException($"{nameof(options.ClientId)} must be provided in options");
			}

			if (string.IsNullOrEmpty(options.ClientSecret))
			{
				throw new InvalidOperationException($"{nameof(options.ClientSecret)} must be provided in options");
			}

			if (string.IsNullOrEmpty(options.UserInfoEndpoint))
			{
				throw new InvalidOperationException($"{nameof(options.UserInfoEndpoint)} must be provided in options");
			}
		}
	}
}