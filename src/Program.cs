using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Get JWT settings from environment variables
var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("JWT Key is missing in environment variables.");
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new InvalidOperationException("JWT Issuer is missing in environment variables.");
var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw new InvalidOperationException("JWT Audience is missing in environment variables.");
var connectionDb = Environment.GetEnvironmentVariable("DataBase__Connection") ?? throw new InvalidOperationException("JWT Audience is missing in environment variables.");


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

// setup the following lines in your startup file
builder.Services.AddCors(options =>
    {

        // you can create multiple policies to use specifically for each controller
        options.AddPolicy("AllowSpecificOrigins", builder =>
        {
            builder.WithOrigins("http://localhost:5125")// Specify the allowed origins
                  .AllowAnyMethod() // Allows all methods
                                    // .WithMethods("GET, POST") // Allows only specific methods
                  .AllowAnyHeader() // Allows all headers
                                    // .WithHeaders("Content-Type", "Authorization") // Allow Specific Headers
                  .AllowCredentials(); // Allows credentials like cookies, authorization headers, etc.
        });
    });


// Configure Entity Framework Core with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(connectionDb));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = " group 5 e-commerce API", Version = "v1" });
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
    Console.WriteLine($"[{DateTime.UtcNow}] [Response]" +
                            $"Status Code: {context.Response.StatusCode}, " +
                            $"Time Taken: {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine($"After Calling: {context.Response.StatusCode}");
});

// Middleware order is important
app.UseHttpsRedirection();
app.UseCors("MyAllowSpecificOrigins");
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