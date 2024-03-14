using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal CartPrice { get; set; }
    }
}