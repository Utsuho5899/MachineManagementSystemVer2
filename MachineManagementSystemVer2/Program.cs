using MachineManagementSystemVer2.Data;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // �ޥ� Cookie ����

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//var app = builder.Build();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- �i�̲׭ץ��j�ϥμз� AddIdentity ��k���N��ʳ]�w ---
// �o�|�۰ʵ��U PasswordHasher, Cookie ����, �H�ΩҦ� Identity �ݭn���A��
builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    // �z�i�H�b�o�̳]�w�K�X��h�A�Ҧp���סB�O�_�ݭn�j�p�g��
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

// �]�w���ε{���� Cookie
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
