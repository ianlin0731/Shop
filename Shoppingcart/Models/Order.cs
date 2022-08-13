using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppingcart.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "收貨人姓名")]
        public string? RecieverName { get; set; }
        [Display(Name = "收貨人電話")]
        public string? RecieverPhone { get; set; }
        [Display(Name = "收貨人地址")]
        public string? RecieverAddress { get; set; }

        public virtual Customer? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
