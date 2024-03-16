using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PaymentService
    {
        private readonly string _stripeSecretKey;

        public PaymentService(string stripeSecretKey)
        {
            _stripeSecretKey = stripeSecretKey;
        }

        public string ProcessPayment(string paymentToken, decimal amount)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new ChargeCreateOptions
            {
                Amount = (int)(amount * 100), // Convert amount to cents
                Currency = "usd",
                Source = paymentToken, // Payment token received from frontend
                Description = "Payment for order",
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            // You can handle the charge response here and return a success message
            return charge.Id; // For example, return the payment charge ID
        }
    }
}
