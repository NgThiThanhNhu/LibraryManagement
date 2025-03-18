using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Service;
using DoAnCuoiKy.Service.InformationLibrary;
using DoAnCuoiKy.Service.IService;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using DoAnCuoiKy.Service.IService.Usermanage;
using DoAnCuoiKy.Service.Usermanage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DoAnCuoiKy.Service.IService.Authentication;
using DoAnCuoiKy.Service.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")//địa chỉ frontend
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

//đăng ký dịch vụ xác thực Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//connectSqlserver
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IBookCategoryService, BookCategoryService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IBookChapterService, BookChapterService>();
builder.Services.AddTransient<IBookItemService, BookItemService>();
builder.Services.AddTransient<IRoleService, RoleService>();

//đăng ký autheticationService 
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
var app = builder.Build();
//connect với frontend
app.UseCors("AllowReactApp"); //kích hoạt CORS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
//kích hoạt jwt


app.MapControllers();

app.Run();
