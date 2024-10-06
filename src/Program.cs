using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShipmentSrvice, ShipmentService>();


// Register your services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configure Entity Framework Core with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Group5 E-commerce API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.Use(async (context, next) =>
{
    var clientIp = context.Connection.RemoteIpAddress?.ToString();
    var stopwatch = Stopwatch.StartNew();
    Console.WriteLine($"[{DateTime.UtcNow}] [Request] " +
                      $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString} " +
                      $"from {clientIp}");

    await next.Invoke();
    stopwatch.Stop();
    Console.WriteLine($"Time Taken: {stopwatch.ElapsedMilliseconds}");
});

// Middleware order is important
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); // Ensure you have this to enable authorization
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Group5 E-commerce V1");
    c.RoutePrefix = string.Empty;
});


app.MapControllers();
app.Run();





// shahad