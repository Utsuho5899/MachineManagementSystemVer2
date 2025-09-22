using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // 引用 Cookie 驗證

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 【最終修正】使用標準 AddIdentity 方法取代手動設定 ---
// 這會自動註冊 PasswordHasher, Cookie 驗證, 以及所有 Identity 需要的服務
builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    // 您可以在這裡設定密碼原則，例如長度、是否需要大小寫等
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

// 設定應用程式的 Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
