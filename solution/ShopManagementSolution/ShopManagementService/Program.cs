﻿
using BusinessObject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
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
            builder.Services.AddScoped<BillDAO>();
            builder.Services.AddScoped<BillDetailDAO>();
            builder.Services.AddScoped<ProductDAO>();
            builder.Services.AddScoped<ReservationDAO>();
            builder.Services.AddTransient<ITableRepository, TableRepository>();
            builder.Services.AddTransient<ITableTypeRepository, TableTypeRepository>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IBillRepository, BillRepository>();
            builder.Services.AddTransient<IBillDetailRepository, BillDetailRepository>();
            builder.Services.AddTransient<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<ShopDAO>();
            builder.Services.AddTransient<IShopRepository, ShopRepositoy>();

            builder.Services.AddScoped<SubscriptionDAO>();
            builder.Services.AddTransient<ISubscriptionRepository, SubscriptionsRepository>();

            // Initialize Supabase client
            var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey);
            supabaseClient.InitializeAsync().Wait();

            // Register Supabase client as a service
            builder.Services.AddSingleton(supabaseClient);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/home/app/.aspnet/DataProtection-Keys"));

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
