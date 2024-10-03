using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

//must be in order
app.UseAuthentication();
app.UseAuthentication();
app.UseSwagger();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();