using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uOW;
        private readonly ApplicationDbContext _context;

        public OrderServices(IUnitOfWork UOW, ApplicationDbContext context)
        {
            _uOW = UOW;
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
        {
            var basket = await _uOW.BasketRepository.GetBasketAsync(basketId);
            var items = new List<OrderItem>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _uOW.ProductRepository.GetByIdAsync(item.Id);
                var productItemOrderd = new ProductItemOrderd(productItem.Id, productItem.Name, productItem.ProductPicture);
                var orderItem = new OrderItem(productItemOrderd, item.Price, item.Quantity);

                items.Add(orderItem);
            }

            await _context.OrderItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            var subTotal = items.Sum(x => x.Quantity * x.Price);

            //get delivery method
            var deliveryMethod = await _context.DeliveryMethods.Where(x => x.Id == deliveryMethodId)
                                .FirstOrDefaultAsync();

            //initilaize on Ctor
            var order = new Order(buyerEmail, shipAddress, deliveryMethod, items, subTotal, basket.PaymentIntentId);

            //check order is not null
            if (order is null) return null;

            //adding order in Db
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            //remove Basket
            await _uOW.BasketRepository.DeleteBasketAsync(basketId);

            return order;

        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _context.Orders
               .Where(x=> x.Id == id)
               .Where(x=>x.BuyerEmail == buyerEmail)
                 .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                    .Include(x => x.DeliveryMethod)
                      .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await _context.Orders
             .Where(x => x.BuyerEmail == buyerEmail)
               .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                  .Include(x => x.DeliveryMethod)
                    .ToListAsync();

            return orders;
        }
    }
}
