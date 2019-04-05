using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ik_word_management.Helper;
using ik_word_management.Models.Domain;
using ik_word_management.Models.JWT;
using ik_word_management.Services.IService;
using ik_word_management.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ik_word_management
{
    public class Startup
    {
        private const string SecretKey = "frQ8VeXeM5e12D6YU3hzKU0KXRHWXvOl";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<JwtFactory>().As<IJwtFactory>();
            containerBuilder.RegisterType<RefreshService>().As<IRefreshService>();
            containerBuilder.RegisterType<UserAccountService>().As<IUserAccountService>();
            containerBuilder.RegisterType<GroupService>().As<IGroupService>();
            
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<IKWordContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServer"), b => b.UseRowNumberForPaging());
                });

            services.AddCors(
                options => options.AddPolicy("AllowSameDomain",
                builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowCredentials()
                )
              );

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            var audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer((configureOption) =>
            {

                configureOption.ClaimsIssuer = issuer;
                configureOption.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                configureOption.SaveToken = true;
            });

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            containerBuilder.Populate(services);
            this.ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors("AllowSameDomain");

            app.UseMvc();

            app.UseWelcomePage();
        }
    }
}
