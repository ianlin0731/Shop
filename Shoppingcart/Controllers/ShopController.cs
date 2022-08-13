using Microsoft.AspNetCore.Mvc;
using Shoppingcart.Models;

namespace Shoppingcart.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopContext _context;
        public ShopController(ShopContext context)
        {
            _context = context;
        }
        //購物網站主畫面
        public IActionResult Index()
        {
            //查詢結果是一個 IQueryable 
            IQueryable<Product> ListAll = from _pTable in _context.Products
                                          select _pTable;

            if (ListAll == null)
            {
                return Content("抱歉！找不到任何一筆記錄");
            }
            else
            {
                return View(ListAll.ToList());  // 執行 .ToList()方法後才真正運作，產生查詢的「結果(result)」。
            }
        }

        //商品詳細資訊
        public IActionResult Details(int id=1003)
        {

            var ListOne = from _p in _context.Products
                                      where _p.ProductId == id
                                      select _p;

            if (ListOne == null)
            {    
                return Content("抱歉！找不到任何一筆記錄");
            }
            else
            {
                return View(ListOne.FirstOrDefault());
            }
        }
    }
}
