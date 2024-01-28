using Core.DTOs.OrderDTOs;
using Core.Entities.OrderAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class OrdersService : IOrdersService
{
    private readonly IProductsRepository productsRepository;
    private readonly ICouponsRepository couponsRepository;
    private readonly IOrdersRepository ordersRepository;


    public OrdersService(IProductsRepository productsRepository, ICouponsRepository couponsRepository, IOrdersRepository ordersRepository)
    {
        this.productsRepository = productsRepository;
        this.couponsRepository = couponsRepository;
        this.ordersRepository = ordersRepository;
    }

    public async Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto)
    {
        // Get total ordered quntity for each product to avoid problems if consumer enter two order items for the same product
        // for example if consumer enter ordersItems=[{productId=1, quntity=5}, {productId=1, quntity=2}], we convert it to [{key=1, value=7}]
        var quntitiesOfOrderdProducts = orderDto.OrderItems
                                                .GroupBy(i => i.ProductId)
                                                .ToDictionary(group => group.Key, group => group.Sum(g => g.Quntity));

        var idsOfOrderdProducts = quntitiesOfOrderdProducts.Keys.ToList();
        var orderedProductsEntities = await productsRepository.GetProductsCollection(idsOfOrderdProducts);

        if (orderedProductsEntities is null || orderedProductsEntities?.Count < idsOfOrderdProducts.Count)
            throw new UnprocessableEntityException("There is one or more invalid product id.");

        // Create order items
        var orderItems = new List<OrderItem>();

        foreach (var product in orderedProductsEntities)
        {
            if (quntitiesOfOrderdProducts[product.Id] > product.Quantity)
                throw new UnprocessableEntityException($"Insufficient stock of '{product.Name}' product.");

            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ImagePath = product.Images.First().Path, // but image may be deleted if the product deleted!! (to be fixed later)
                Price = product.Price,
                Quntity = quntitiesOfOrderdProducts[product.Id]
            });

            product.Quantity -= quntitiesOfOrderdProducts[product.Id];
            productsRepository.UpdateProduct(product);
        }

        // Calculate order price
        var subtotal = orderItems.Sum(i => i.Quntity * i.Price);

        double discount = 0;
        if (!string.IsNullOrEmpty(orderDto.CouponCode))
        {
            var coupon = await couponsRepository.GetCoupon(orderDto.CouponCode);
            discount = coupon?.Value ?? 0;
        }

        double shippingCosts = 50.0; //it is fixed now regardless of the shipping address (may be changed later)

        // Create the order
        var order = new Order
        {
            CustomerEmail = customerEmail,
            Phone = orderDto.Phone,
            ShippingAddress = new Address(orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.City),
            OrderItems = orderItems,
            Subtotal = subtotal,
            ShippingCosts = shippingCosts,
            Discount = discount
        };

        ordersRepository.AddOrder(order);
        await ordersRepository.SaveChanges();
        await productsRepository.SaveChanges(); //doesnt has meaning because the changes already saved in the previous line (so refactor to unit of work later)

        return order.Id;
    }

}
