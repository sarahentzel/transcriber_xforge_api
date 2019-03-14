using System;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JsonApiDotNetCore.Extensions;
using SIL.XForge.Configuration;
using SIL.Transcriber.Services;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;

namespace transcriber_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public IContainer ApplicationContainer { get; private set; }

        private bool IsDevelopment => Environment.IsDevelopment() || Environment.IsEnvironment("Testing");

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            services.AddConfiguration(Configuration);

            services.AddExceptionLogging();

            services.AddCommonServices();

            //services.AddXFIdentityServer(Configuration, IsDevelopment);

            //var siteOptions = Configuration.GetOptions<SiteOptions>();
            /*
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    if (IsDevelopment)
                    {
                        options.RequireHttpsMetadata = false;
                        options.BackchannelHttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback
                                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    }
                    options.Authority = siteOptions.Origin.ToString();
                    options.Audience = "api";
                });
            */
            services.AddTranscriberDataAccess(Configuration);


            var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            /*  original easy api...
            services.Configure<Settings>(options =>
            {
                options.ConnectionString
                    = System.Environment.GetEnvironmentVariable("ConnectionString") ??
                      Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = System.Environment.GetEnvironmentVariable("Database") ??
                      Configuration.GetSection("MongoConnection:Database").Value;
            });
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITaskServiceJSON, TaskServiceJSON>();
            */
            services.AddTranscriberJsonApi(mvcBuilder, containerBuilder, Configuration);

            containerBuilder.Populate(services);

            ApplicationContainer = containerBuilder.Build();
            return new AutofacServiceProvider(ApplicationContainer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseJsonApi();
            app.UseTranscriberDataAccess();
        }
    }
}
