using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using EventManager.Datalayer;
using EventManager.Datalayer.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace EventManager.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      var configuration = builder.Configuration;

      ConfigureServices(builder.Services, configuration, builder.Environment);

      var app = builder.Build();

      using (var scope = app.Services.CreateScope())
      {
        var dbContext = scope.ServiceProvider.GetRequiredService<EventManagerDbContext>();
        dbContext.Database.Migrate();
      }

      ConfigureMiddleware(app, builder.Environment);

      app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
      services.AddDbContext<EventManagerDbContext>(options =>
          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

      services.AddSingleton<IDboConverter, DboConverter>();
      services.AddScoped<IAddressRepository, AddressRepository>();
      services.AddScoped<IEventRepository, EventRepository>();
      services.AddScoped<IUserEventRepository, UserEventRepository>();
      services.AddScoped<IUserRepository, UserRepository>();

      services.Configure<RequestLocalizationOptions>(options =>
      {
        options.DefaultRequestCulture = new RequestCulture("pl-PL");
      });

      services.AddControllers().AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      });

      services.AddEndpointsApiExplorer();

      services.AddCors(options =>
      {
        options.AddPolicy("CustomPolicy", builder =>
        {
          builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
        });
      });

      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("1.0", new OpenApiInfo
        {
          Title = "EventManager",
          Description = "Aplikacja do zarz¹dzania wydarzeniami",
          Version = "1.0"
        });

        if (env.IsDevelopment() || Debugger.IsAttached)
        {
          options.ExampleFilters();
        }

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        options.EnableAnnotations();
        options.DescribeAllParametersInCamelCase();
      });

      services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
    }


    private static void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
    {
      app.UseCors("CustomPolicy");
      app.UseRouting();

      if (env.IsDevelopment() || Debugger.IsAttached)
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint("/swagger/1.0/swagger.json", "EventManager 1.0");
          options.EnableFilter();
          options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
          options.DisplayOperationId();
        });
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      app.MapControllers();
    }
  }
}
