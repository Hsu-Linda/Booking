using Booking.Models;
using Booking.Repositories;
using Booking.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//驗證服務
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //驗證處理常式
    .AddJwtBearer(options =>
    {
        //詳細配置
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            //是否使用密鑰驗證簽名
            ValidateIssuerSigningKey = true,
            //驗證Jwt簽名的密鑰 對稱同一隻
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            //是否驗證簽發者
            ValidateIssuer = false,
            //是否驗證受眾
            ValidateAudience = false
        };
    });
builder.Services.AddMemoryCache();
builder.Services.AddMvcCore().AddDataAnnotations();
builder.Services.AddDbContext<BookingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingDatabase")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<ActivityRepository>();
builder.Services.AddScoped<TicketTypeService>();
builder.Services.AddScoped<TicketTypeRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TicketRepository>();

builder.Services.AddControllers().AddNewtonsoftJson(option =>
    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
