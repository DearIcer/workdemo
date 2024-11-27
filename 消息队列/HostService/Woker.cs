using NewLife.Redis.Core;
using 消息队列.MessageModel;

namespace 消息队列.HostService;

public class Woker : BackgroundService
{
    private readonly INewLifeRedis  _lifeRedis;
    private readonly ILogger<Woker> _logger;
    public Woker(INewLifeRedis lifeRedis, ILogger<Woker> logger)
    {
        _lifeRedis = lifeRedis;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _lifeRedis.AddQueue("General", new RedisMessageModel());
            await Task.Delay(100, stoppingToken);
        }
    }
}