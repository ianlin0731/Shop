using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoppingcart.Models;
using System.Security.Claims;

namespace Shoppingcart.Controllers
{
    public class ManageOrderController : Controller
    {
        private readonly ShopContext _context;

        public ManageOrderController(ShopContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Order(IFormCollection _form)
        {
            //讀取會員的ID，才能個人資料寫入訂單主檔（Order）
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

            //連結DB Cart資料表。把這一名會員以前留在購物車的紀錄，一次結帳。
            var ListAll = from m in _context.Carts
                          where m.UserId == Convert.ToInt32(_ID)
                          select m;

            Order _order = new Order
            {
                UserId = Convert.ToInt32(_ID),
                RecieverName = _form["RecieverName"],
                RecieverPhone = _form["RecieverPhone"],
                RecieverAddress = _form["RecieverAddress"],
            };

            foreach (var _cart in ListAll)
            {
                //  在訂單明細檔中加入多項產品（原本放在購物車裡面的）
                _order.OrderDetails.Add(new OrderDetail
                {
                    OrderId = _order.OrderId,
                    Name = _cart.Name,
                    Quantity = _cart.Quantity,
                    Price = _cart.Price,
                }); ;
            }

            _context.Orders.Add(_order);

            // 一旦購物車的商品全部結帳，就把購物車裡面的紀錄清空
            // 必須先鎖定、先找到這筆記錄。找得到，才能刪除！
            var origins = (from po in _context.Carts
                           where po.UserId == Convert.ToInt32(_ID)
                           select po).ToList();

            _context.Carts.RemoveRange(origins);  // 批次  刪除多筆記錄。

            _context.SaveChanges();

            //return Content("新增訂單成功");
            return RedirectToAction("MyOrder", "ManageOrder");//跳轉到我的訂單
        }

        //網站管理員看出貨訂單
        [Authorize(Roles = "Administrator")]
        public IActionResult OrderList()
        {
            var ListAll = from o in _context.Orders
                          select o;

            return View(ListAll.ToList());
        }
        //網站管理員看出貨訂單明細
        [Authorize(Roles = "Administrator")]
        public IActionResult OrderDetails(int id)
        {
            var ListAll = from o in _context.OrderDetails
                          where o.OrderId == id
                          select o;

            return View(ListAll.ToList());
        }

        //客戶看訂單
        [Authorize]
        public IActionResult MyOrder()
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
            var ListAll = from c in _context.Orders
                          where c.UserId == ID
                          select c;

            return View(ListAll.ToList());
        }

        //客戶看訂單明細
        [Authorize]
        public IActionResult MyOrderDeatils(int id)
        {
            var ListAll = from c in _context.OrderDetails
                          where c.OrderId == id
                          select c;

            return View(ListAll.ToList());
        }
    }
}
