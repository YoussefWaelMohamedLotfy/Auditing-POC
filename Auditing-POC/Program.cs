using Audit.Core;
using Audit.Elasticsearch.Providers;
using Audit.WebApi;
using Auditing_POC.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

Configuration.Setup()
    .UseElasticsearch(config => config
        .ConnectionSettings(new AuditConnectionSettings(new Uri("http://raspberrypi:9200")))
        .Index(auditEvent => $"appauditlogs-{auditEvent.EventType.ToLower()}"));

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithEnvironmentUserName()
        //.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://raspberrypi:9200"))
        {
            IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower()}-{context.HostingEnvironment.EnvironmentName?.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = true,
            FailureCallback = e => Console.WriteLine("Unable to submit event: " + e.MessageTemplate),
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
            NumberOfReplicas = 1,
            NumberOfShards = 2
        })
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContextPool<AppDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuditMiddleware(_ => _
//    .WithEventType("{verb}:{url}")
//    .IncludeHeaders()
//    .IncludeResponseHeaders()
//    .IncludeRequestBody()
//    .IncludeResponseBody());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
