// <copyright file="LandSenseExtensions.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication
{
	using System;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;
	using Microsoft.Extensions.Options;

	public static class LandSenseExtensions
	{
		public static AuthenticationBuilder AddLandSense(this AuthenticationBuilder builder) =>
			builder.AddLandSense(LandSenseDefaults.AuthenticationScheme, options => { });

		public static AuthenticationBuilder AddLandSense(this AuthenticationBuilder builder, Action<LandSenseOptions> configureOptions) =>
			builder.AddLandSense(LandSenseDefaults.AuthenticationScheme, configureOptions);

		public static AuthenticationBuilder AddLandSense(this AuthenticationBuilder builder, string authenticationScheme,
			Action<LandSenseOptions> configureOptions) =>
			builder.AddLandSense(authenticationScheme, null, configureOptions);

		public static AuthenticationBuilder AddLandSense(this AuthenticationBuilder builder, string authenticationScheme,
			string displayName, Action<LandSenseOptions> configureOptions)
		{
			builder.Services.TryAddEnumerable(ServiceDescriptor
				.Singleton<IPostConfigureOptions<LandSenseOptions>, LandSensePostConfigureOptions>());
			return builder.AddScheme<LandSenseOptions, LandSenseHandler>(authenticationScheme, displayName, configureOptions);
		}
	}
}