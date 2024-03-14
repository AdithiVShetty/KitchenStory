namespace KitchenStory.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal CartPrice { get; set; }

        //public virtual Food Food { get; set; }
        //public virtual User User { get; set; }
    }
}