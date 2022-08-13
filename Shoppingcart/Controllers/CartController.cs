using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shoppingcart.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shoppingcart.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext _context;
        //資料庫連線
        public CartController(ShopContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Cart()
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.User;
            int ID = 0;
            if (null != principal)
            {
                foreach (Claim claim in principal.Claims)
                {
                    if (claim.Type == "SelfDefine_ID")
                        ID = Convert.ToInt32(claim.Value);
                }
            }
            var ListAll = from c in _context.Carts
                          where c.UserId == ID
                          select c;

            return View(ListAll.ToList());
        }

        //刪除購物車
        [Authorize]
        public IActionResult Delete(int id)
        {
            Cart item = _context.Carts.Find(id);

            if (item == null)
            {
                TempData["Error"] = "此筆資料不存在";
            }
            else
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();

                TempData["Success"] = "刪除成功";
            }
            return RedirectToAction("Cart");
        }

        //產品加入購物車
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CartCookie(IFormCollection _form)
        {
            // 讀取會員的ID，才能個人資料寫入訂單主檔（Order）
            string _ID = "";
            ClaimsPrincipal principal = (ClaimsPrincipal)HttpContext.User;
            if (null != principal)
            {
                foreach (Claim claim in principal.Claims)
                {
                    if (claim.Type == "SelfDefine_ID")
                        _ID = claim.Value;
                }
            }
            else
            {
                return Content("抱歉！找不到這位用戶的ID");
            }

            //資料寫入購物車
            Cart _cart = new Cart
            {
                ProductId = Convert.ToInt32(_form["ProductID"]),
                Name = _form["Name"],
                Price = Convert.ToDecimal(_form["Price"]),
                Quantity = Convert.ToInt32(_form["Quantity"]),
                Amount = Convert.ToDecimal(_form["Price"]) * Convert.ToInt32(_form["Quantity"]),
                UserId = Convert.ToInt32(_ID)
            };

            //資料庫儲存
            _context.Carts.Add(_cart);
            _context.SaveChanges();

            //測試資料↓↓↓
            //string result = _cart.ProductId + "***" + _cart.Name+ "***"+_cart.Price+ "***" + _cart.Quantity + "***" +_cart.Amount+"***" + _cart.UserId;                                    
            //return Content(result);
            return RedirectToAction("Cart", "Cart"); //寫入後跳轉至購物車
        }
    }
}
