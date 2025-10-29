using LoginModel;
using LoginService.Interfaces;
using LoginModel.MTConsumerEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LoginAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(
    ILogger<LoginController> logger,
    IRedisCacheService cacheService,
    IPublishEndpoint publishEndpoint
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] LoginRequest request)
    {
        var correlationId = Guid.NewGuid().ToString();

        logger.LogInformation("Received login for {User}", request.Username);

        var cacheKey = $"login:{correlationId}";
        await cacheService.SetCacheValueAsync(cacheKey, request, TimeSpan.FromMinutes(10));
        logger.LogInformation("SavedToRedis {Key}", cacheKey);

        await publishEndpoint.Publish<LoginEvent>(new
        {
            CorrelationId = correlationId,
            request.Username,
            request.Ip,
            request.UserAgent,
            request.Succeeded
        });

        logger.LogInformation("Published login event {CorrelationId}", correlationId);

        return Ok(new { correlationId });
    }
}
