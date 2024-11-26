using DataAccess.DAO;
using DataAccess.IRepository.Repository;

namespace OwnerServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var supabaseConfig = builder.Configuration.GetSection("Supabase");
            var supabaseUrl = supabaseConfig["Url"];
            var supabaseKey = supabaseConfig["ApiKey"];


            // Initialize Supabase client
            var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey);
            supabaseClient.InitializeAsync().Wait();

            // Register Supabase client as a service
            builder.Services.AddSingleton(supabaseClient);

            builder.Services.AddTransient<OwnerDAO>();
            builder.Services.AddTransient<OwnerRepository>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
