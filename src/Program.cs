using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddDbContext<AppDBContext>(options =>
//  options.UseNpgsql(builder.Configuration.GetConnectionString
//  ("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();