
using BusinessObject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShopManagementService.DAO;
using ShopManagementService.IRepositories;
using ShopManagementService.Repositories;
using System.Text;

namespace ShopManagementService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            var supabaseConfig = builder.Configuration.GetSection("Supabase");
            var supabaseUrl = supabaseConfig["Url"];
            var supabaseKey = supabaseConfig["ApiKey"];

            builder.Services.AddAuthorization();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
                    ValidAudience = builder.Configuration["Authentication:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JWTSecret"]!))
                };
            });

            builder.Services.AddScoped<TableDAO>();
            builder.Services.AddScoped<TableTypeDAO>();
            builder.Services.AddTransient<ITableRepository, TableRepository>();
            builder.Services.AddScoped<BillDAO>();
            builder.Services.AddScoped<BillDetailDAO>();
            builder.Services.AddScoped<ProductDAO>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();

            // Initialize Supabase client
            var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey);
            supabaseClient.InitializeAsync().Wait();

            // Register Supabase client as a service
            builder.Services.AddSingleton(supabaseClient);

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
