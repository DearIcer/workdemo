using Microsoft.AspNetCore.Mvc;
using NewLife.Redis.Core;

namespace redisIOC.Controller;
[Route("api/[controller]/[action]")]
[ApiController]
public class DemoController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly INewLifeRedis  _lifeRedis;
    public DemoController(INewLifeRedis lifeRedis)
    {
        _lifeRedis = lifeRedis;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        _lifeRedis.Set("Ioc", "111111111111");
        return Content(_lifeRedis.Get<string>("Ioc"));
    }
}