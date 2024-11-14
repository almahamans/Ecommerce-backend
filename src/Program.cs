using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt; // Corrected from JWT to Jwt
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models; // Added for OpenApiInfo and OpenApiSecurityScheme

var builder = WebApplication.CreateBuilder(args);

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:5173/") // Specify the allowed origins
              .AllowAnyMethod() // Allows all methods
              .AllowAnyHeader() // Allows all headers
              .AllowCredentials(); // Allows credentials like cookies, authorization headers, etc.
    });
});

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Get JWT settings from environment variables
var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new InvalidOperationException("JWT Key is missing in environment variables.");
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new InvalidOperationException("JWT Issuer is missing in environment variables.");
var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw new InvalidOperationException("JWT Audience is missing in environment variables.");
var connectionDb = Environment.GetEnvironmentVariable("DataBase__Connection") ?? throw new InvalidOperationException("Database connection string is missing in environment variables.");

System.Console.WriteLine($"Connection DB {connectionDb}");

// Configure Entity Framework Core with Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionDb));

System.Console.WriteLine("DB successfully");

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShipmentSrvice, ShipmentService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Group5 E-commerce API", Version = "v1" });
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var key = Encoding.ASCII.GetBytes(jwtKey); // Corrected to use jwtKey directly
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtIssuer, // Corrected to use jwtIssuer directly
        ValidAudience = jwtAudience, // Corrected to use jwtAudience directly
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

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
});

// Middleware order is important
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins"); // Corrected to match the defined CORS policy
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