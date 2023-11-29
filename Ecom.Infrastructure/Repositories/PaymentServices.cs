using Azure.Core;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _uOW;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public PaymentServices(IUnitOfWork UOW, IConfiguration config, ApplicationDbContext context)
        {
            _uOW = UOW;
            _config = config;
            _context = context;
        }

        public async Task<CustomerBasket> CreateOrUpdatePayment(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:Secretkey"];
            var basket = await _uOW.BasketRepository.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket == null) return null;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _context.DeliveryMethods.Where(x => x.Id == basket.DeliveryMethodId.Value).FirstOrDefaultAsync();

                shippingPrice = deliveryMethod.Price;


            }

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _uOW.ProductRepository.GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }
            var paymentIntents = new PaymentIntentService();
           
                var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
                {
                    Amount = (long)23 * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }

                    // Additional options can be set here
                });


                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            
          
            await _uOW.BasketRepository.UpdateBasketAsync(basket);
            return basket;


        }
    }
}
