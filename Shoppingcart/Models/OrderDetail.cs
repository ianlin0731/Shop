using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppingcart.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        [Display(Name = "商品名稱")]
        public string? Name { get; set; }
        [Display(Name = "價格")]
        public decimal? Price { get; set; }
        [Display(Name = "購買數量")]
        public int? Quantity { get; set; }

        public virtual Order? Order { get; set; }
    }
}
