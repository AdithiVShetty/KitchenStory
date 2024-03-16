namespace BusinessLogicLayer
{
    public class PaymentIntentCreateDTO
    {
        public long Amount { get; set; } // Amount in cents (e.g., $10.00 = 1000 cents)
        public string Currency { get; set; } // Currency code (e.g., USD, EUR)
        public string Description { get; set; }
    }
}