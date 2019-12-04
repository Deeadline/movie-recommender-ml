using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Recommend_Movie_System.Filters;
using Recommend_Movie_System.Models.Entity;
using Recommend_Movie_System.Models.Response;
using Recommend_Movie_System.Repository;
using Recommend_Movie_System.Services;
using Recommend_Movie_System.Services.Interface;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Recommend_Movie_System.Configuration
{
    public static class StartupExtension
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
        }

        public static void ConfigureFilters(this IServiceCollection services)
        {
            services.AddScoped<EntityExistAttribute<User>>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IMovieCommentService, MovieCommentService>();
            services.AddScoped<IMovieFeedbackService, MovieFeedbackService>();
            services.AddScoped<IMovieRecommendationService, MovieRecommendationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info()
                {
                    Version = "v1",
                    Title = "JuniorStart API",
                    Description = "Documentation for API",
                    TermsOfService = null
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(
                options => { options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")); },
                ServiceLifetime.Transient);
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            byte[] key = Encoding.UTF8.GetBytes(configuration.GetSection("JWT").GetSection("SecretKey").Value);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            int userId = int.Parse(context.Principal.Identity.Name);
                            var user = userService.Get(userId);
                            if (user == null)
                            {
                                context.Fail("Unauthorized");
                            }

                            return Task.FromResult(0);
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void EnableSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recommend Movie System API V1");
                c.RoutePrefix = string.Empty;
            });
        }

        public static void ExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var exception = error.Error;

                        await context.Response.WriteAsync(
                            new ErrorResponse(context.Response.StatusCode, exception.Message).ToString());
                    }
                });
            });
        }
    }
}