using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.application.Repositories;
using news_aggregator.application;
using news_aggregator.domain.Models;
using news_aggregator.infrastructure.Repositories;
using news_application.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using news_aggregator.infrastructure.Jobs;

namespace news_aggregator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<NewsDataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IExternalSourceRepository, ExternalSourceRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IExternalSourceService, ExternalSourceService>();
            builder.Services.AddScoped<INewsService, NewsService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();

            builder.Services.AddHttpClient<IExternalNewsClient, ExternalNewsClient>();

            builder.Services.AddScoped<INewsQueryService, NewsQueryService>();

            builder.Services.Configure<NewsApiOptions>(
                builder.Configuration.GetSection("ExternalApis:NewsApi"));

            builder.Services.AddScoped<ISavedArticleRepository, SavedArticleRepository>();
            builder.Services.AddScoped<ISavedArticleService, SavedArticleService>();

            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<INotificationConfigRepository, NotificationConfigRepository>();
            builder.Services.AddScoped<INotificationConfigService, NotificationConfigService>();

            builder.Services.AddScoped<IUserKeywordRepository, UserKeywordRepository>();
            builder.Services.AddScoped<IUserKeywordService, UserKeywordService>();  

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddHttpClient<NewsApiProvider>();
            builder.Services.AddHttpClient<AltApiProvider>();
            builder.Services.AddScoped<INewsProviderFactory, NewsProviderFactory>();

            builder.Services.AddHostedService<NewsFetcherJob>();



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
