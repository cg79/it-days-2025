
using dapper_implementation;
using dapper_simple_crud_implementation;
using ef_base_repository;
using ef_implementation;
using linq_to_db_implementation;

namespace ef_dapper
{
    public class Startup(IConfiguration configuration)
    {
        private IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterRepositories(services);
            ServiceRegistration.RegisterServices(services, Configuration);

            services.AddControllers(options =>
            {
                // options.Filters.AddService<CaptureResponseActionFilter>();
            });
           
            services.AddCors();
        }
        private static IServiceCollection RegisterRepositories(IServiceCollection services)
        {
            // services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<UserServiceEF>();
            services.AddScoped<UserService_Dapper>();
            services.AddScoped<UserService_SimpleCrud>();
            services.AddScoped<UserService_LinqToDb>();
            // services.AddScoped<IUserServiceFactory, UserServiceFactory>();
            
            // services.AddScoped<IUserServiceLinqToDb, UserService_LinqToDb>();
            // services.AddScoped<IUserRepository, UserRepository>();
            // services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
            return services;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}