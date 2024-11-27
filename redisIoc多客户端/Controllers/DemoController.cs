using Microsoft.AspNetCore.Mvc;
using NewLife.Redis.Core;


namespace redisIoc多客户端.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class Worker : Controller
{
    private readonly INewLifeRedis _newLifeRedis;
    private readonly ILogger<Worker> _logger;
    public Worker(ILogger<Worker> logger, IRedisCacheManager redisCacheManager)
    {
        _logger = logger;
        _newLifeRedis = redisCacheManager.GetRedis("1");
        _newLifeRedis.Set("ice", "ice");
        _newLifeRedis = redisCacheManager.GetRedis("2");
        _newLifeRedis.Set("ice", "ice");
        //支持动态添加和删除
        redisCacheManager.AddRedis(new RedisConfig { Name = "test", ConnectionString = "xx" });
        redisCacheManager.RemoveRedis("test");
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        _newLifeRedis.Set("Ioc", "111111111111");
        // return Content(_newLifeRedis.Get<string>("Ioc"));
        return Ok();
    }
    
    [HttpGet]
    public IActionResult Three()
    {
        List<string> list = new List<string>();
        list.Add("jack");
        list.Add("tom");
        list.Add("lucy");
        list.Add("jack");
        list.Add("tom");
        string key = "list";
        foreach (var item in list)
        {
            _newLifeRedis.SetAdd(key, item);
        }

        var result = _newLifeRedis.SetGetAll<string>(key);
        return Ok();
    }

    [HttpGet]
    public IActionResult SortedSetTest()
    {
        _newLifeRedis.SortedSetAdd("sortedset", "jack", 1);
        _newLifeRedis.SortedSetAdd("sortedset", "tom", 2);
        _newLifeRedis.SortedSetAdd("sortedset", "lucy", 3);
        _newLifeRedis.SortedSetAdd("sortedset", "jack2", 4);
        _newLifeRedis.SortedSetAdd("sortedset", "tom3", 5);

        var result = _newLifeRedis.GetRedisSortedSet<string>("sortedset").Range(0, -1);
        return Ok();
    }
}