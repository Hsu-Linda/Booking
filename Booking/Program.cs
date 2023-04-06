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

//���ҪA��
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //���ҳB�z�`��
    .AddJwtBearer(options =>
    {
        //�ԲӰt�m
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            //�O�_�ϥαK�_����ñ�W
            ValidateIssuerSigningKey = true,
            //����Jwtñ�W���K�_ ��٦P�@��
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            //�O�_����ñ�o��
            ValidateIssuer = false,
            //�O�_���Ҩ���
            ValidateAudience = false
        };
    });
builder.Services.AddMemoryCache();
builder.Services.AddMvcCore().AddDataAnnotations();
builder.Services.AddDbContext<BookingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingDatabase")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<OpenSqlService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<ActivityRepository>();
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
