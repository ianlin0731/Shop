using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Shoppingcart.Models;
using System.Security.Claims;

namespace Shoppingcart.Controllers
{
    public class LoginDBController : Controller
    {
        private readonly ShopContext _context;

        public LoginDBController(ShopContext context)
        {
            _context = context;
        }

        //登入畫面
        public IActionResult Login()
        {
            return View();
        }

        //登入
        [HttpPost]
        [ValidateAntiForgeryToken]   // 避免XSS、CSRF攻擊
        public ActionResult Login(Customer _User)
        {
            if (ModelState.IsValid)
            {
                var ListOne = from m in _context.Customers
                              where m.UserName == _User.UserName && m.UserPassword == _User.UserPassword
                              select m;
                Customer? _result = ListOne.FirstOrDefault();  // 執行上面的查詢句，得到 "第一筆" 結果。

                if (_result == null)
                {
                    ViewData["ErrorMessage"] = "帳號與密碼有錯";
                    return View();
                }
                else
                {
                    var claims = new List<Claim>   // 搭配 System.Security.Claims; 命名空間
                    {
                        new Claim(ClaimTypes.Name, _User.UserName),
                        // 在登入畫面上輸入的「帳號」。
                        // 讀取後的結果是「CLAIM TYPE: http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name; CLAIM VALUE: 你輸入的數值」

                        new Claim("SelfDefine_ID", _result.UserId.ToString()),    // 購物車會用到 Customer ID
                        // 透過登入畫面上輸入的「帳號」，在Customer資料表找到獨一無二的ID - CustomerId。

                        new Claim("SelfDefine_LastName", _User.UserName),    
                        // [自己定義的屬性]  讀取後的結果是「CLAIM TYPE: SelfDefine_LastName; CLAIM VALUE: 你輸入的數值」

                        new Claim(ClaimTypes.Role, "Administrator")   
                        // *** 如果要有「群組、角色、權限」，可以加入這一段 *********
                    };

                    // 登入 Login 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),   // 從現在算起，Cookie何時過期
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    // 登入 Login
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            new ClaimsPrincipal(claimsIdentity),
                                                            authProperties);

                    //return Content("<h3>登入成功</h3>");
                    return RedirectToAction("Index", "Shop");
                }
            }
            return View();
        }

        //登出
        public async Task<IActionResult> Logout()
        {
            // Microsoft.AspNetCore.Authentication.Cookies; 命名空間
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "LoginDB");
        }

    }
}
