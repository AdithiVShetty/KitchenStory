using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class CartItemDTO
    {
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
