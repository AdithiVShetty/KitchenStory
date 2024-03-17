using BusinessLogicLayer;
using KitchenStory.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace KitchenStory.Controllers
{
    public class PaymentsController : ApiController
    {
        private readonly PaymentService _paymentService;
        public PaymentsController()
        {
            _paymentService = new PaymentService("stripe-secret-key");
        }

        [HttpPost]
        [Route("api/payments/processpayment")]
        public IHttpActionResult ProcessPayment([FromBody] PaymentModel paymentRequest)
        {
            try
            {
                string paymentId = _paymentService.ProcessPayment(paymentRequest.TransactionId, paymentRequest.Amount);
                return Ok(new { PaymentId = paymentId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/payments/createpaymentintent")]
        public async Task<IHttpActionResult> CreatePaymentIntent([FromBody] PaymentIntentCreateDTO paymentIntentDto)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentIntentDto.Amount,
                Currency = paymentIntentDto.Currency,
                PaymentMethodTypes = new List<string> { "card" },
                Description = paymentIntentDto.Description
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return Ok(new { clientSecret = intent.ClientSecret });
        }
    }
}