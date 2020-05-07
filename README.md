# Introduction

This is a fork of [SaaSKit](https://github.com/saaskit/).

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

As the name suggests, Dime.Owin.MultiTenancy adds a pipeline to the OWIN middleware which is concerned with resolving tenants from the (web) requests. The pipeline is separated from the process of resolving the tenant. Tenants can be resolved in many different ways. A few examples include subdomains, cookies, query string parameters, etc.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Owin.MultiTenancy:

`dotnet add package Dime.Owin.MultiTenancy`

If you want to have access to the pipeline from HttpContext, add the following package:

`dotnet add package Dime.Owin.MultiTenancy.Mvc5`

## Usage

``` csharp

using Owin.MultiTenancy;

[assembly: OwinStartup(typeof(Startup))]
namespace MyApp 
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      app.UseMultiTenancy(new TenantSubdomainResolver());
    }
  }

  public class TenantSubdomainResolver : UriTenantResolver<Tenant>
  {
     private readonly ITenantService _tenantService; // Injected service

     public override async t.Task<TenantContext<Tenant>> ResolveAsync(Uri uri)
     {
          string subdomain = uri.GetSubdomain(); // Extension method to parse the uri and fetch the subdomain
          Tenant tenant = await _tenantService.Get(subdomain);

          return tenant != null ? new TenantContext<Tenant>(tenant) : default;
     }
  }
}

```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)