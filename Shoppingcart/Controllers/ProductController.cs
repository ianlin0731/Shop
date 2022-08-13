using Microsoft.AspNetCore.Mvc;
using Shoppingcart.Models;

namespace Shoppingcart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopContext _context;
        public ProductController(ShopContext context)
        {
            _context = context;
        }

        public IActionResult List()
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

        //新增商品
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product _product, IFormFile FileUpload_FileName)
        {
            string strMessage = "";
            string _FileName = "";

            #region            
            try
            {
                if (FileUpload_FileName.Length > 0)   // 檢查 <input type="file"> 是否輸入檔案？
                {
                    _FileName = Path.GetFileName(FileUpload_FileName.FileName);  
                    string _path = "d:\\temp\\uploads\\";      // 或是寫成  string _path = @"d:\temp\uploads\";
                
                    // 透過 Path.Combine()處理目錄與路徑「斜線」問題  
                    string _end = Path.Combine(_path, _FileName);

                    using (var stream = System.IO.File.Create(_end))
                    {
                        FileUpload_FileName.CopyTo(stream);
                    }

                    // 上傳後，檔案存放在Web Server的硬碟裡面，需要「（目錄）路徑」、「檔名」兩大條件！

                    strMessage += "上傳成功，請檢查 (1) FileUpload_DB資料表與 (2)上傳的目錄。<br>完整路徑與檔名：" + _end;
                }
                else
                    strMessage += "您尚未挑選檔案，無法上傳";
            }
            catch
            {
                strMessage += "上傳失敗。File upload failed!!";
            }
            #endregion

            // 存入資料表以後，檔名變成 System.Web.HttpPostedFileWrapper
            // https://www.codeproject.com/Questions/1180504/Image-name-save-as-system-web-httppostedfilewrappe
            _product.FileUploadFileName = _FileName;

            if (ModelState.IsValid)
            {
                _context.Products.Add(_product);
                _context.SaveChanges();
                TempData["Success"] = "新增成功";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Success"] = "新增失敗";
            }
            return View();
        }

        //修改商品
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
            }

            Product item = _context.Products.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return View(item);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product _product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(_product);
                _context.SaveChanges();

                TempData["Success"] = "修改成功";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "修改失敗";
            }
            return View();
        }

        //刪除商品
        public IActionResult Delete(int id)
        {
            Product item = _context.Products.Find(id);

            if (item == null)
            {
                TempData["Error"] = "此筆資料不存在";
            }
            else
            {
                _context.Products.Remove(item);
                _context.SaveChanges();

                TempData["Success"] = "刪除成功";
            }
            return RedirectToAction("List");
        }
    }
}
