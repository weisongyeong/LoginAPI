using LoginConsumer.Consumers;
using LoginData;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// Configure Serilog to read from appsettings.json
builder.Services.AddSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

// Add DB connection
builder.Services.AddDbContext<LoginDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LoginDB")));

// Add RabbitMQ configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoginEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        cfg.ReceiveEndpoint("login-events", e =>
        {
            e.ConfigureConsumer<LoginEventConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
