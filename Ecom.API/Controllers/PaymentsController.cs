using Ecom.API.Errors;
using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentsController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentServices.CreateOrUpdatePayment(basketId);
            if (basket == null) return BadRequest(new BaseCommonResponse(400, "Problem With Your Basket"));

            return basket;
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "whsec_b3e73d05307ea95f82feabbda7d5fdb2ea33cf3b70e5b33496f943bb5414e724";

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
