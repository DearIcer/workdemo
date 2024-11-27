using NewLife.Redis.Core;

var redisCacheManager = new RedisCacheManager(new List<RedisConfig>
    { new RedisConfig { Name = "1", ConnectionString = "server=192.168.0.180:7000;password=8icymZp_WvFkzMt;db=2" } });
redisCacheManager.AddRedis(new RedisConfig
    { Name = "2", ConnectionString = "server=192.168.0.180:6379;password=8icymZp_WvFkzMt;db=0" });
//支持动态添加和删除
redisCacheManager.AddRedis(new RedisConfig
    { Name = "test", ConnectionString = "server=192.168.0.180:8000;password=8icymZp_WvFkzMt;db=0" });
redisCacheManager.RemoveRedis("test");
var redis = redisCacheManager.GetRedis("2");
//普通操作
redis.Set("test", "1");
Console.WriteLine(redis.Get<string>("test"));
//列表
redis.ListAdd("listtest", 1);
redis.ListGetAll<string>("listtest");
//SortedSet
redis.SortedSetAdd("sortsettest", "1", 1.0);
redis.SortedSetIncrement("sortsettest", "1", 1.0);
//set
redis.SetAdd("settest", "2");
//哈希
redis.HashAdd("hashtest", "1", "2");
redis.HashGet<string>("hashtest", new string[] { "1" });
//队列操作
//方式1
var queue = redis.GetRedisQueue<string>("queue");
queue.Add("test");
var data = queue.Take(1);
//方式2
redis.AddQueue("queue", "1");
redis.GetQueueOne<string>("queue");