namespace BusinessLogicLayer
{
    public class PlaceOrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public string Status { get; set; }
    }
}