using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PatientPortal.Common;
using PatientPortal.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PatientPortal
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public string Secret_Key { get; }
        public static SymmetricSecurityKey SigningKey { get; set;}
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            Secret_Key = _configuration.GetValue<string>(Constants.AuthorizationSecret);
            SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret_Key));
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Enable CORS  --- This it to disable to security and allow all requests from different domain
            services.AddCors(c =>
            {
                c.AddPolicy(Constants.AllowOrigin, options => options.SetIsOriginAllowed(hostname => Constants.True).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddControllers();
            services.AddScoped<IPatient, Patient>();
            services.AddSingleton<IDictionary, Dictionary<int, string>>();
            //Validating the jwt token
            services.AddAuthentication(options => { options.DefaultAuthenticateScheme = Constants.JwtBearer; options.DefaultChallengeScheme = Constants.JwtBearer; })
                .AddJwtBearer(Constants.JwtBearer, jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        IssuerSigningKey = SigningKey,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Constants.PatientPortal,
                        ValidAudience = Constants.PatientPortalUI,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(60)
                    };
                });
            //Added swagger for running the service locally
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.SwaggerVersion, new OpenApiInfo { Title = Constants.CurrentNameSpace, Version = Constants.SwaggerVersion, Description = Constants.CurrentDescription });
                var xmlFile = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, Constants.XmlExtension);
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(Constants.AllowOrigin);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(Constants.SwaggerJson, string.Concat(Constants.CurrentNameSpace, Constants.Space, Constants.SwaggerVersion)));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
