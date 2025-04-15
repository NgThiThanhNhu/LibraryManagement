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
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")//địa chỉ frontend
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
        });
});

//đăng ký dịch vụ xác thực Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

// Cấu hình Cookie Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gửi cookie qua HTTPS
    options.Cookie.SameSite = SameSiteMode.None; // Cho phép cookie cross-site
});

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
builder.Services.AddTransient<IBookImportTransactionService, BookImportTransactionService>();
builder.Services.AddHttpContextAccessor();


//đăng ký autheticationService 
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
var app = builder.Build();
//connect với frontend
app.UseCors("AllowAll"); //kích hoạt CORS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//hỗ trợ đọc token từ cookie trước khi xác thực
app.Use(async (context, next) =>
{
    if (context.Request.Cookies.TryGetValue("token", out var token))
    {
        context.Request.Headers.Append("Authorization", $"Bearer {token}");
    }
    await next();
});
//xác thực và đăng nhập
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
