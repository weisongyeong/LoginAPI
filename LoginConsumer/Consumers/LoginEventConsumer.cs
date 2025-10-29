using LoginModel.MTConsumerEvents;
using LoginData;
using LoginData.Models;
using MassTransit;

namespace LoginConsumer.Consumers;

public class LoginEventConsumer(
    ILogger<LoginEventConsumer> logger,
    LoginDbContext db
    ) : IConsumer<LoginEvent>
{
    public async Task Consume(ConsumeContext<LoginEvent> context)
    {
        var msg = context.Message;

        logger.LogInformation("Consumed event {CorrelationId}", msg.CorrelationId);

        var exists = db.LoginLogs.Any(l => l.CorrelationId == msg.CorrelationId);
        if (exists)
        {
            logger.LogWarning("Duplicate ignored {CorrelationId}", msg.CorrelationId);
            return;
        }

        db.LoginLogs.Add(new LoginLog
        {
            Id = Guid.NewGuid(),
            CorrelationId = msg.CorrelationId,
            Username = msg.Username,
            Ip = msg.Ip,
            UserAgent = msg.UserAgent,
            Succeeded = msg.Succeeded,
            Timestamp = msg.Timestamp
        });
        await db.SaveChangesAsync();

        logger.LogInformation("Saved login log {CorrelationId}", msg.CorrelationId);
    }
}

