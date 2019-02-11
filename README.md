# SpatialFocus.AspNetCore.LandSenseAuthentication

Spatial Focus ASP.NET Core LandSense Authentication Handler

[![NuGet](https://img.shields.io/nuget/v/SpatialFocus.AspNetCore.LandSenseAuthentication.svg)](https://www.nuget.org/packages/SpatialFocus.AspNetCore.LandSenseAuthentication/)

## Installation

```console
Install-Package SpatialFocus.AspNetCore.LandSenseAuthentication
```

## Setup & configuration

This extension a simple method for an API, that restricts certain endpoints to users with a valid [LandSense](https://landsense.eu) access token. To set up the application you first need to register your service in the [LandSense Authorization Server](https://as.landsense.eu). Provide the client-id and client-secret in the appsettings.json file like in the following code sample.

```json
"LandSenseCredentials": {
   "ClientId": "12345678-abcd-1234-12ab-123456789abc@as.landsense.eu",
   "ClientSecret": "1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"
}
```

Register the LandSense Authentication Handler in the `Startup.cs`.

```csharp
using SpatialFocus.AspNetCore.LandSenseAuthentication;

public class Startup
{
   // ...

   public void Configure(IApplicationBuilder app, IHostingEnvironment env)
   {
      // ...

      app.UseAuthentication();

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
```

## Example usage

For an exemplary usage for a "secret" endpoint that prints the claims for the user account referenced with the Bearer access-token, see the `SampleController` in the [sample project](https://github.com/SpatialFocus/SpatialFocus.AspNetCore.LandSenseAuthentication/master/sample/SpatialFocus.AspNetCore.LandSenseAuthentication.ApiSample).

```csharp
[Authorize]
[HttpGet("secret")]
public ActionResult<string> Secret(int id)
{
   IEnumerable<string> claims = 
      HttpContext?.User?.Claims.Select(x => $"{x.Type} => {x.Value}") ?? new List<string>();

   return "Congratulations!" + Environment.NewLine +
      "Claims:" + Environment.NewLine +
      string.Join(Environment.NewLine, claims);
}
```

----

Made with :heart: by [Spatial Focus](https://spatial-focus.net/)
