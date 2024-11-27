using NewLife.Redis.Core;
using 消息队列.MessageModel;

namespace 消息队列.HostService;

public class Customer1 : BackgroundService
{
    private readonly INewLifeRedis _lifeRedis;

    public Customer1(INewLifeRedis lifeRedis)
    {
        _lifeRedis = lifeRedis;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = _lifeRedis.GetQueue<RedisMessageModel>("General", 1);
            if (data.Count > 0)
            {
                data.ForEach(msg => { Console.WriteLine($"Custome1 收到消息:{msg.Id} {msg.Data}"); });
            }
            else
            {
                Console.WriteLine($"Custome1 没有消息");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}