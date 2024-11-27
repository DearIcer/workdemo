using NewLife.Redis.Core;
using 消息队列.HostService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNewLifeRedis();
builder.Services.AddHostedService<Woker>();
builder.Services.AddHostedService<Customer1>();
builder.Services.AddHostedService<Customer2>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.Run();

