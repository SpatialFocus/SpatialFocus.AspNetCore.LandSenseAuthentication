// <copyright file="Startup.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication.ApiSample
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseAuthentication();

			app.UseHttpsRedirection();
			app.UseMvc();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(options => { options.DefaultScheme = LandSenseDefaults.AuthenticationScheme; })
				.AddLandSense(options =>
				{
					options.UserInfoEndpoint = "https://as.landsense.eu/oauth/userinfo";
					options.ClientId = Configuration["LandSenseCredentials:ClientId"];
					options.ClientSecret = Configuration["LandSenseCredentials:ClientSecret"];
				});
			services.AddAuthorization();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}
	}
}