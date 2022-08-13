using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shoppingcart.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//連結資料庫（DB連結字串），讀取設定檔的內容
builder.Services.AddDbContext<ShopContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

//Cookie
builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                // 以下這兩個設定可有可無
                options.AccessDeniedPath = "/LoginDB/AccessDeny";   // 拒絕，不允許登入會跳到這一頁。
                options.LoginPath = "/LoginDB/Login";     // 登入頁。

                options.Cookie.Name = "Ian"; //預設值AspNetCore Cookie
                //https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.builder.cookieauthenticationoptions.cookiename?view=aspnetcore-1.1
                options.Cookie.HttpOnly = true;
            });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//登入會用到
app.UseAuthentication();   // 驗證
// 設定 HttpContext.User 屬性，並針對要求執行授權中介軟體。      
app.UseAuthorization();  // 授權

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
