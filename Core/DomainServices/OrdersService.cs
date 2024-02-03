using AutoMapper;
using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;
using Stripe;
using Address = Core.Entities.OrderAggregate.Address;

namespace Core.DomainServices;

public class OrdersService : IOrdersService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICouponsService couponsService;
    private readonly IMapper mapper;

    public OrdersService(IUnitOfWork unitOfWork, ICouponsService couponsService, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.couponsService = couponsService;
        this.mapper = mapper;
    }

    public async Task<PagedResult<OrderForAdminListDto>> GetOrders(OrderQueryParameters queryParams)
    {
        var pageOfOrdersEntities = await unitOfWork.Orders.GetOrders(queryParams);

        var pageOfOrderstDtos = mapper.Map<PagedResult<Order>, PagedResult<OrderForAdminListDto>>(pageOfOrdersEntities);

        return pageOfOrderstDtos;
    }

    public async Task<PagedResult<OrderForCustomerListDto>> GetCustomerOrders(string customerEmail, PaginationQueryParameters queryParams)
    {
        var pageOfOrdersEntities = await unitOfWork.Orders.GetCustomerOrders(customerEmail, queryParams);

        var pageOfOrderstDtos = mapper.Map<PagedResult<Order>, PagedResult<OrderForCustomerListDto>>(pageOfOrdersEntities);

        return pageOfOrderstDtos;
    }

    public async Task<OrderDetailsDto> GetOrder(int id)
    {
        var orderEntity = await unitOfWork.Orders.GetOrder(id);
        if (orderEntity is null)
            throw new NotFoundException($"Order not found.");

        var orderDto = mapper.Map<Order, OrderDetailsDto>(orderEntity);
        return orderDto;
    }

    public async Task<OrderDetailsDto> GetCustomerOrder(string customerEmail, int orderId)
    {
        //this method is for customer view to allow logged-in customer to get its orders only unlike the GetOrder method which is for admin view and allow admin to get any order
        var order = await GetOrder(orderId);

        if (order.CustomerEmail != customerEmail)
            throw new ForbiddenException("You are not authorized to get this order.");

        return order;
    }

    public async Task<int> CreateOrder(string customerEmail, OrderForCreatingDto orderDto)
    {
        // Get total ordered quntity for each product to avoid problems if consumer enter two order items for the same product
        // for example if consumer enter ordersItems=[{productId=1, quntity=5}, {productId=1, quntity=2}], we convert it to [{key=1, value=7}]
        var quntitiesOfOrderdProducts = orderDto.OrderItems
                                                .GroupBy(i => i.ProductId)
                                                .ToDictionary(group => group.Key, group => group.Sum(g => g.Quntity));

        var idsOfOrderdProducts = quntitiesOfOrderdProducts.Keys.ToList();
        var orderedProductsEntities = await unitOfWork.Products.GetProductsCollection(idsOfOrderdProducts);

        if (orderedProductsEntities is null || orderedProductsEntities?.Count < idsOfOrderdProducts.Count)
            throw new UnprocessableEntityException("There is one or more invalid product id.");

        // Create order items
        var orderItems = new List<OrderItem>();

        orderedProductsEntities?.ForEach(product =>
        {
            if (product.Quantity < quntitiesOfOrderdProducts[product.Id])
                throw new UnprocessableEntityException($"Insufficient stock of '{product.Name}' product.");

            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quntity = quntitiesOfOrderdProducts[product.Id]
            });

            product.Quantity -= quntitiesOfOrderdProducts[product.Id];
            unitOfWork.Products.UpdateProduct(product);
        });

        // Calculate order price
        var subtotal = orderItems.Sum(i => i.Quntity * i.Price);
        double shippingCosts = 50.0; //it is fixed now regardless of the shipping address (may be changed later)

        // Create the order
        var order = new Order
        {
            CustomerEmail = customerEmail,
            CustomerPhone = orderDto.CustomerPhone,
            ShippingAddress = new Address(orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.City),
            OrderItems = orderItems,
            Subtotal = subtotal,
            ShippingCosts = shippingCosts
        };

        //apply the discount (if there is any)
        if (!string.IsNullOrEmpty(orderDto.CouponCode))
            order.Discount = await couponsService.CalculateCouponDiscount(order.Subtotal, orderDto.CouponCode);

        unitOfWork.Orders.AddOrder(order);
        await unitOfWork.SaveChanges();

        await CreatePaymentIntent(order);

        return order.Id;
    }

    public async Task UpdateOrderStatus(int id, OrderStatus newOrderStatus)
    {
        var order = await unitOfWork.Orders.GetOrder(id);
        if (order is null)
            throw new NotFoundException($"Order not found.");

        //return quntity back to the products if the order failed or canceled
        if (newOrderStatus == OrderStatus.Failed || newOrderStatus == OrderStatus.Canceled)
        {
            var productsOrderdQuntity = order.OrderItems.ToDictionary(item => item.ProductId, item => item.Quntity);

            var productOrderdIds = productsOrderdQuntity.Keys.ToList();

            var productEntities = await unitOfWork.Products.GetProductsCollection(productOrderdIds);

            productEntities?.ForEach(product =>
            {
                product.Quantity += productsOrderdQuntity[product.Id];
                unitOfWork.Products.UpdateProduct(product);
            });
        }

        order.Status = newOrderStatus;

        unitOfWork.Orders.UpdateOrder(order);
        await unitOfWork.SaveChanges();
    }

    private async Task CreatePaymentIntent(Order order)
    {
        StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)order.Total * 100,
            Currency = "egp",
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string> { { "orderId", order.Id.ToString() } }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        order.PaymentClientSecret = paymentIntent.ClientSecret;

        unitOfWork.Orders.UpdateOrder(order);
        await unitOfWork.SaveChanges();
    }

}