using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shoppingcart.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//�s����Ʈw�]DB�s���r��^�AŪ���]�w�ɪ����e
builder.Services.AddDbContext<ShopContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

//Cookie
builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                // �H�U�o��ӳ]�w�i���i�L
                options.AccessDeniedPath = "/LoginDB/AccessDeny";   // �ڵ��A�����\�n�J�|����o�@���C
                options.LoginPath = "/LoginDB/Login";     // �n�J���C

                options.Cookie.Name = "Ian"; //�w�]��AspNetCore Cookie
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

//�n�J�|�Ψ�
app.UseAuthentication();   // ����
// �]�w HttpContext.User �ݩʡA�ðw��n�D������v�����n��C      
app.UseAuthorization();  // ���v

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
