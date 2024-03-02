using Microsoft.EntityFrameworkCore;
using Skills.Models;
using Skills.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Skills.Validators;



namespace Skills
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {   
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<SkillValidator>(ServiceLifetime.Transient);
            
            services.AddTransient<IPersonRepository, PersonRepository>();

            string connectionString = Configuration.GetConnectionString("PostgresConnection");
            services.AddDbContext<SkillContext>(options =>
                options.UseNpgsql(connectionString));
                
            services.AddControllers();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app)
        {   
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseAuthentication();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapControllers();
            });

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
        }
    }
}
