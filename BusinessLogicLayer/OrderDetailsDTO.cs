using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class OrderDetailsDTO
    {
        public List<CartDTO> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public UserDTO UserDetails { get; set; }
        public PaymentMode PaymentMode { get; set; }
    }
}
