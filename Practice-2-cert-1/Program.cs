using Microsoft.OpenApi.Models;
using Serilog;
using UPB.BusinessLogic.Managers;
using UPB.BusinessLogic.Managers.Exceptions;
using UPB.Practice_2_cert_1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// **** Add environment configuration
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile(
        "appsettings." +  Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json"
    )
    .Build();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<PatientManager>();

// **** Retrieve the log filepath
string? logFilepath = builder.Configuration.GetSection("FilePaths").GetSection("LogFile").Value;

if(logFilepath == null)
{
    throw new JSONValueNotFoundException(["FilePaths", "LogFile"]);
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// **** Retrieving the Title
string? title = builder.Configuration.GetSection("AppInfo").GetSection("AppName").Value;

if(title == null)
{
    throw new JSONValueNotFoundException(["AppInfo", "AppName"]);
}

// **** Retrieving the Version
string? version = builder.Configuration.GetSection("AppInfo").GetSection("Version").Value;

if(version == null)
{
    throw new JSONValueNotFoundException(["AppInfo", "Version"]);
}

// **** Setting the Title to the one configured at the appsettings file of the current environment
builder.Services.AddSwaggerGen(options => 
    {
        options.SwaggerDoc(builder.Configuration.GetSection("AppInfo").GetSection("Version").Value, new OpenApiInfo()
        {
            Title = title,
            Version = version
        }
            );
    }
);

var app = builder.Build();

// **** Especifying use of ExceptionHandlerMiddleware
app.UseExceptionHandlerMiddleware();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

// **** Selecting wether to write logs to a file and console or only a file
if (app.Environment.IsDevelopment())
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File(logFilepath, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}
else if (app.Environment.EnvironmentName.Equals("QA"))
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File(logFilepath, rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

// **** Retrieving the tab title
string? tabTitle = builder.Configuration.GetSection("AppInfo").GetSection("TabText").Value;

if(tabTitle == null)
{
    throw new JSONValueNotFoundException(["AppInfo", "TabText"]);
}

Log.Information("Initializing WebAPI...");

// **** Start the UI without worrying about the environment in use
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.DocumentTitle = tabTitle;
});

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
