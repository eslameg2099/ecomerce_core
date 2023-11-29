﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class OrderItemDto
    {
        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
