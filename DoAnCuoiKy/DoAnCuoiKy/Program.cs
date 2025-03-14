using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Service;
using DoAnCuoiKy.Service.InformationLibrary;
using DoAnCuoiKy.Service.IService;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using DoAnCuoiKy.Service.IService.Usermanage;
using DoAnCuoiKy.Service.Usermanage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
