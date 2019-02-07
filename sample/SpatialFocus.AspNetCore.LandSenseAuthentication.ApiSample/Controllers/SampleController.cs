// <copyright file="SampleController.cs" company="Spatial Focus">
// Copyright (c) Spatial Focus. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace SpatialFocus.AspNetCore.LandSenseAuthentication.ApiSample.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Route("/")]
	[ApiController]
	public class SampleController : ControllerBase
	{
		[HttpGet]
		public ActionResult<string> Default()
		{
			return "This endpoint is unprotected." + Environment.NewLine +
				"But in order to request /secret you need to provide a valid LandSense access token as bearer token.";
		}

		[Authorize]
		[HttpGet("secret")]
		public ActionResult<string> Secret(int id)
		{
			IEnumerable<string> claims = HttpContext?.User?.Claims.Select(x => $"{x.Type} => {x.Value}") ?? new List<string>();
			return "Congratulations!" + Environment.NewLine + "Claims:" + Environment.NewLine + string.Join(Environment.NewLine, claims);
		}
	}
}