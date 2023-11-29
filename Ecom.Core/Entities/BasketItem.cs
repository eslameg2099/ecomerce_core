using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductPicture { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }

    }
}
