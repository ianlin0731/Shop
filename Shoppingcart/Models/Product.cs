using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shoppingcart.Models
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Display(Name = "商品名稱")]
        public string? Name { get; set; }
        [Display(Name = "商品介紹")]
        public string? Description { get; set; }
        [Display(Name = "價格")]
        public decimal? Price { get; set; }
        [Display(Name = "商品製造日期")]
        [DataType(DataType.Date)]
        public DateTime? PublishDate { get; set; }
        [Display(Name = "商品圖片")]
        public string? FileUploadFileName { get; set; }
        [Display(Name = "庫存")]
        public int? Quantity { get; set; }
    }
}
