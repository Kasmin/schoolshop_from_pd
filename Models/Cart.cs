﻿using System;
using System.Collections.Generic;

namespace Shop.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public List<CartItem> CartItems { get; set; }


        public Cart()
        {
        }
    }
}
