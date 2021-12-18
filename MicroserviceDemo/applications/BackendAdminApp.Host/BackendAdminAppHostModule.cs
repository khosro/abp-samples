using System;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProductManagement;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using MsDemo.Shared;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.OAuth;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.UI.Navigation;
using Volo.Blogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

using ProductManagement;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using MsDemo.Shared;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Blogging;

namespace BackendAdminApp.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        
        
    //    typeof(AbpEntityFrameworkCoreSqlServerModule),

         typeof(AbpPermissionManagementEntityFrameworkCoreModule)//Autofac.Core.DependencyResolutionException: None of the constructors found with 'Volo.Abp.Autofac.AbpAutofacConstructorFinder' on type 'Volo.Abp.PermissionManagement.PermissionStore' can be invoked with the available services and parameters:
        , typeof(AbpPermissionManagementApplicationModule)//Autofac.Core.DependencyResolutionException: An exception was thrown while activating Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement.PermissionManagementModal. --->Autofac.Core.DependencyResolutionException: None of the constructors found with 'Volo.Abp.Autofac.AbpAutofacConstructorFinder' on type 'Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement.Permis
        , typeof(AbpPermissionManagementDomainIdentityModule)// Volo.Abp.AbpException: No policy defined to get/set permissions for the provider 'U'. Use PermissionManagementOptions to map the policy.
      // , typeof(AbpPermissionManagementDomainIdentityServerModule)// does not need
      //,  typeof(AbpPermissionManagementHttpApiModule)// does not need

      , typeof(AbpIdentityHttpApiClientModule)//An internal error occurred during your request! ---  None of the constructors found with 'Volo.Abp.Autofac.AbpAutofacConstructorFinder' on type 'Volo.Abp.Identity.IdentityUserController' can be invoked with the available services and parameters:
      , typeof(AbpHttpClientIdentityModelWebModule)//Authorization failed! Given policy has not granted.
      , typeof(AbpIdentityWebModule)// access to /Identity/Users and ...
                                    //  , typeof(AbpIdentityHttpApiModule)// does not need



        //,typeof(AbpTenantManagementApplicationContractsModule),
        //typeof(AbpTenantManagementHttpApiModule),
        //typeof(AbpTenantManagementHttpApiClientModule),
        //typeof(AbpTenantManagementEntityFrameworkCoreModule),


        , typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpFeatureManagementHttpApiModule)
        , typeof(AbpFeatureManagementHttpApiClientModule)

        //typeof(AbpSettingManagementEntityFrameworkCoreModule)


        , typeof(AbpAspNetCoreMvcUiBasicThemeModule)
     //   , typeof(AbpAspNetCoreMvcUiMultiTenancyModule)

       //, typeof(BloggingApplicationContractsModule)
       // , typeof(ProductManagementHttpApiModule)


      )]
    public class BackendAdminAppHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MsDemoConsts.IsMultiTenancyEnabled;
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new BackendAdminAppMenuContributor(configuration));
            });

            context.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(365);
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.ClientId = configuration["AuthServer:ClientId"];
                    options.ClientSecret = configuration["AuthServer:ClientSecret"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("role");
                    options.Scope.Add("email");
                    options.Scope.Add("phone");
                    options.Scope.Add("BackendAdminAppGateway");
                    options.Scope.Add("IdentityService");
                    options.Scope.Add("ProductService");
                    options.Scope.Add("TenantManagementService");

                });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });


            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Admin Application API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "MsDemo-DataProtection-Keys");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            if (MsDemoConsts.IsMultiTenancyEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseAbpRequestLocalization();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Admin Application API");
            });
            app.UseConfiguredEndpoints();
        }
    }
}
