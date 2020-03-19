using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Diagnostics.AspNetCore;
using Google.Cloud.Diagnostics.Common;
using MacedonianCroatianEnglishGermanTranslateV3.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MacedonianCroatianEnglishGermanTranslateV3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            var logger = new LoggerFactory().CreateLogger("ConfigureDevelopmentServices");
            services.Configure<GoogleCloudPlatformProjectName>(options => {
                options.ProjectName = Environment.GetEnvironmentVariable("ProjectName");
            });
            services.Configure<GoogleTranslateApiCredentials>(options =>
            {
                options.auth_provider_x509_cert_url = Environment.GetEnvironmentVariable("auth_provider_x509_cert_url");
                options.auth_uri = Environment.GetEnvironmentVariable("auth_uri");
                options.client_email = Environment.GetEnvironmentVariable("client_email");
                options.client_id = Environment.GetEnvironmentVariable("client_id");
                options.client_x509_cert_url = Environment.GetEnvironmentVariable("client_x509_cert_url");
                options.private_key = Configuration.GetSection("private_key").Value;
                options.private_key_id = Environment.GetEnvironmentVariable("private_key_id");
                options.project_id = Environment.GetEnvironmentVariable("project_id");
                options.token_uri = Environment.GetEnvironmentVariable("token_uri");
                options.type = Environment.GetEnvironmentVariable("type");
            });
            /*services.Configure<GoogleReCaptcha>(options =>
            {
                options.ClientKey = Environment.GetEnvironmentVariable("data-sitekey");
                options.SecretKey = Environment.GetEnvironmentVariable("secret");
            });*/
            services.Configure<GoogleReCaptchaV3>(options =>
            {
                options.v3_site_key = Environment.GetEnvironmentVariable("v3_site_key");
                options.v3_secret = Environment.GetEnvironmentVariable("v3_secret");
            });

            services.Configure<OxfordDictionariesApi>(options =>
            {
                options.app_id = Environment.GetEnvironmentVariable("app_id");
                options.app_key = Environment.GetEnvironmentVariable("app_key");    
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.Configure<GoogleCloudPlatformProjectName>(options =>
            {
                options.ProjectName = Environment.GetEnvironmentVariable("project_id");
            });

            GoogleCloudPlatformSecretManager googleCloudPlatformSecretManager = new GoogleCloudPlatformSecretManager();

            /*services.Configure<GoogleReCaptcha>(async options =>
            {
                JObject jObjectGoogleReCaptcha = JsonConvert.DeserializeObject<JObject>(await googleCloudPlatformSecretManager.getSecretManagerValues(Environment.GetEnvironmentVariable("project_id"),
                    Environment.GetEnvironmentVariable("v2_recaptcha_secret_id"), Environment.GetEnvironmentVariable("v2_recaptcha_secret_version_id")));
                options.ClientKey = jObjectGoogleReCaptcha["GoogleReCaptcha"]["ClientKey"].ToString();
                options.SecretKey = jObjectGoogleReCaptcha["GoogleReCaptcha"]["SecretKey"].ToString();
            });*/
            services.Configure<GoogleReCaptchaV3>(async options =>
            {
                JObject jObjectGoogleReCaptchaV3 = JsonConvert.DeserializeObject<JObject>(await googleCloudPlatformSecretManager.getSecretManagerValues(Environment.GetEnvironmentVariable("project_id"),
                    Environment.GetEnvironmentVariable("recaptcha_secret_id_v3"), Environment.GetEnvironmentVariable("recaptcha_secret_version_id_v3")));
                options.v3_site_key = jObjectGoogleReCaptchaV3["GoogleReCaptchaV3"]["v3_website_key"].ToString();
                options.v3_secret = jObjectGoogleReCaptchaV3["GoogleReCaptchaV3"]["v3_secret_key"].ToString();
            });

            services.Configure<OxfordDictionariesApi>(async options =>
            {
                JObject jObjectGoogleReCaptchaV3 = JsonConvert.DeserializeObject<JObject>(await googleCloudPlatformSecretManager.getSecretManagerValues(Environment.GetEnvironmentVariable("project_id"),
                    Environment.GetEnvironmentVariable("oxford_dictionaries_api"), Environment.GetEnvironmentVariable("oxford_dictionaries_api_version_id")));
                options.app_id = jObjectGoogleReCaptchaV3["OxfordDictionariesApi"]["app_id"].ToString();
                options.app_key = jObjectGoogleReCaptchaV3["OxfordDictionariesApi"]["app_key"].ToString();
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if (env.IsProduction())
            {
                // Configure logging service.
                loggerFactory.AddGoogle(app.ApplicationServices, Environment.GetEnvironmentVariable("project_id"));
            }
            var logger = loggerFactory.CreateLogger("Logger");
            // Write the log entry.
            logger.LogInformation("Logger started. This is a log message.");
            // Configure error reporting service.
            //app.UseGoogleExceptionLogging();
            // Configure trace service.
            //app.UseGoogleTrace();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
