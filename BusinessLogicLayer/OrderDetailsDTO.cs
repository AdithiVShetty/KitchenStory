using System.Collections.Generic;

namespace BusinessLogicLayer
{
    public class OrderDetailsDTO
    {
        public List<CartDTO> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public UserDTO UserDetails { get; set; }
        public string PaymentMode { get; set; }
    }
}