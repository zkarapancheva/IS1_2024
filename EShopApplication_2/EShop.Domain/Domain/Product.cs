﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class Product : BaseEntity
    {

        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductDescription { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
        public virtual ICollection<ProductInShoppingCart>? ProductInShoppingCarts { get; set; }

    }
}
