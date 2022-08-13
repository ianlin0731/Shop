using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shoppingcart.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        [Display(Name = "帳號(UserName)")]
        [Required(ErrorMessage = "必填欄位")]
        public string? UserName { get; set; }
        [Display(Name = "密碼(Password)")]
        [Required(ErrorMessage = "必填欄位")]
        [DataType(DataType.Password)]
        public string? UserPassword { get; set; }
        public int? UserRank { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
