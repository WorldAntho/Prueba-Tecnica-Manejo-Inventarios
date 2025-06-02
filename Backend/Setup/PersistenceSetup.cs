using Backend.Common.Core.Helpers;
using Backend.Common.Core.Persistence;
using Backend.Common.Core.Wrapper;
using Backend.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;


namespace Backend.Setup
{
    public static class PersistenceSetup
    {
        private const int MaxDbCommandTimeout = 30;
        private const string JwtSettingsSection = "JWTSettings";
        private const string DbContextSettingsSection = "DbContextSettings";

        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDatabaseContext(configuration)
                .AddRepositories()
                .AddAuthenticationConfig(configuration);

            return services;
        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(DbContextSettingsSection);
            var loggingEnabled = dbSettings.GetValue<bool>("Logging");
            var enableDetailedErrors = dbSettings.GetValue<bool>("EnableDetailedErrors");
            var enableSensitiveDataLogging = dbSettings.GetValue<bool>("EnableSensitiveDataLogging");
            var dbExecutionTimeout = Math.Min(dbSettings.GetValue<int>("TimeOut"), MaxDbCommandTimeout);

            services.AddDbContext<gestioninventariosContext>(options =>
            {
                ConfigureDbContextOptions(options, configuration.GetConnectionString("Context"),
                    loggingEnabled, enableDetailedErrors, enableSensitiveDataLogging, dbExecutionTimeout);
            }, ServiceLifetime.Transient);

            return services;
        }

        private static void ConfigureDbContextOptions(DbContextOptionsBuilder options, string? connectionString,
            bool loggingEnabled, bool detailedErrors, bool sensitiveDataLogging, int timeout)
        {
            if (loggingEnabled)
            {
                options.EnableDetailedErrors(detailedErrors)
                       .EnableSensitiveDataLogging(sensitiveDataLogging);
            }

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
            {
                mysqlOptions.CommandTimeout(timeout);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLazyLoadingProxies();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var repositoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(IRepository).IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

            foreach (var repository in repositoryTypes)
            {
                services.AddTransient(repository);
            }

            services.AddSingleton<AppSettings>();
            services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            return services;
        }
        private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(JwtSettingsSection).Get<AppSettings>()
                ?? throw new InvalidOperationException("JWT Settings are missing in configuration");

            if (string.IsNullOrEmpty(jwtSettings.Secret))
            {
                throw new InvalidOperationException("JWT Secret key is missing in configuration");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                ConfigureJwtBearerOptions(options, jwtSettings);
            });

            return services;
        }

        private static void ConfigureJwtBearerOptions(JwtBearerOptions options, AppSettings jwtSettings)
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context => HandleJwtChallenge(context),
                OnForbidden = context => HandleJwtForbidden(context)
            };
        }

        private static Task HandleJwtChallenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(Response.Failure("Usted no está autorizado", HttpStatusCode.Unauthorized));
            return context.Response.WriteAsync(result);
        }

        private static Task HandleJwtForbidden(ForbiddenContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(Response.Failure("Usted no tiene permisos sobre este recurso", HttpStatusCode.Forbidden));
            return context.Response.WriteAsync(result);
        }
    }
}