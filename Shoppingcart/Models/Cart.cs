using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppingcart.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "商品名稱")]
        public string? Name { get; set; }
        [Display(Name = "價格")]
        public decimal? Price { get; set; }
        [Display(Name = "購買數量")]
        public int? Quantity { get; set; }
        [Display(Name = "小計")]
        public decimal? Amount { get; set; }
        public int? UserId { get; set; }

        public virtual Customer? User { get; set; }
    }
}
