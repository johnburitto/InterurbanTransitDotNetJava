using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Data;
using Security.Enteties;
using Security.Seeds;
using Security.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
      "[{Timestamp:HH:mm:ss} {Level}][{SourceContext}] {Message:lj}{NewLine}{Exception}",
      theme: AnsiConsoleTheme.Code)
    .Enrich.WithProperty("Environment", hostContext.HostingEnvironment.EnvironmentName)
    .ReadFrom.Configuration(hostContext.Configuration);
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration["MSSQLServer"]));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.EmitStaticAudienceClaim = true;
})
    //.AddInMemoryApiResources(Config.ApiResources)
    //.AddInMemoryApiScopes(Config.ApiScopes)
    //.AddInMemoryClients(Config.Clients)
    //.AddInMemoryIdentityResources(Config.IdentityResources)
    .AddConfigurationStore(options => options.ConfigureDbContext = db => db.UseSqlServer(builder.Configuration["MSSQLServer"], assembly => assembly.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
    .AddOperationalStore(options => options.ConfigureDbContext = db => db.UseSqlServer(builder.Configuration["MSSQLServer"], assembly => assembly.MigrationsAssembly(typeof(Config).Assembly.GetName().Name)))
    .AddAspNetIdentity<AppUser>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseIdentityServer();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapControllers();

if(args.Contains("/seed"))
{
    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app);
    Log.Information("Done seeding database. Exiting");

    return;
}

app.Run();
