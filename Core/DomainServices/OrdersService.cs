using AutoMapper;
using Core.DTOs.OrderDTOs;
using Core.DTOs.QueryParametersDTOs;
using Core.Entities.OrderAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Address = Core.Entities.OrderAggregate.ShippingAddress;

namespace Core.DomainServices;

public class OrdersService : IOrdersService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ICouponsService couponsService;
    private readonly IPaymentService paymentService;
    private readonly IMapper mapper;

    public OrdersService(IUnitOfWork unitOfWork, ICouponsService couponsService, IPaymentService paymentService, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.couponsService = couponsService;
        this.paymentService = paymentService;
        this.mapper = mapper;
    }

    public async Task<PagedResult<OrderForAdminListResponse>> GetOrders(OrderQueryParameters queryParams)
    {
        var pageOfOrdersEntities = await unitOfWork.Orders.GetOrders(queryParams);

        var pageOfOrderstDtos = mapper.Map<PagedResult<Order>, PagedResult<OrderForAdminListResponse>>(pageOfOrdersEntities);

        return pageOfOrderstDtos;
    }

    public async Task<PagedResult<OrderForCustomerListResponse>> GetCustomerOrders(Guid customerId, PaginationQueryParameters queryParams)
    {
        var pageOfOrdersEntities = await unitOfWork.Orders.GetCustomerOrders(customerId, queryParams);

        var pageOfOrderstDtos = mapper.Map<PagedResult<Order>, PagedResult<OrderForCustomerListResponse>>(pageOfOrdersEntities);

        return pageOfOrderstDtos;
    }

    public async Task<OrderResponse> GetOrder(Guid id)
    {
        var orderEntity = await unitOfWork.Orders.GetOrder(id);
        if (orderEntity is null)
            throw new NotFoundException($"Order not found.");

        var orderDto = mapper.Map<Order, OrderResponse>(orderEntity);
        return orderDto;
    }

    public async Task<OrderResponse> GetCustomerOrder(Guid customerId, Guid orderId)
    {
        //this method is for customer view to allow logged-in customer to get its orders only unlike the GetOrder method which is for admin view and allow admin to get any order
        var order = await GetOrder(orderId);

        if (order.CustomerId != customerId)
            throw new ForbiddenException("You are not authorized to get this order.");

        return order;
    }

    public async Task<Guid> CreateOrder(Guid customerId, OrderAddRequest orderAddRequest)
    {
        // Get total ordered quntity for each product to avoid problems if consumer enter two order items for the same product
        // for example if consumer enter ordersItems=[{productId=1, quntity=5}, {productId=1, quntity=2}], we convert it to [{key=1, value=7}]
        var quntitiesOfOrderdProducts = orderAddRequest.OrderLines
                                                .GroupBy(i => i.ProductId)
                                                .ToDictionary(group => group.Key, group => group.Sum(g => g.Quntity));

        var idsOfOrderdProducts = quntitiesOfOrderdProducts.Keys.ToList();
        var orderedProductsEntities = await unitOfWork.Products.GetProductsCollection(idsOfOrderdProducts);

        if (orderedProductsEntities is null || orderedProductsEntities?.Count < idsOfOrderdProducts.Count)
            throw new UnprocessableEntityException("There is one or more invalid product id.");

        // Create order items
        var orderItems = new List<OrderLine>();

        orderedProductsEntities?.ForEach(product =>
        {
            if (product.Quantity < quntitiesOfOrderdProducts[product.Id])
                throw new UnprocessableEntityException($"Insufficient stock of '{product.Name}' product.");

            orderItems.Add(new OrderLine
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
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            ShippingAddress = new Address(orderAddRequest.ShippingAddress.AddressLine, orderAddRequest.ShippingAddress.City),
            OrderLines = orderItems,
            Subtotal = subtotal,
            ShippingCosts = shippingCosts
        };

        //apply the discount (if there is any)
        if (!string.IsNullOrEmpty(orderAddRequest.CouponCode))
            order.Discount = await couponsService.CalculateCouponDiscount(order.Subtotal, orderAddRequest.CouponCode);

        //create a payment intent
        order.PaymentIntentClientSecret = await paymentService.CreatePaymentIntent(order.Id, order.Total);

        unitOfWork.Orders.AddOrder(order);
        await unitOfWork.SaveChanges();

        return order.Id;
    }

    public async Task UpdateOrderStatus(Guid id, OrderStatus newOrderStatus)
    {
        var order = await unitOfWork.Orders.GetOrder(id);
        if (order is null)
            throw new NotFoundException($"Order not found.");

        //return quntity back to the products if the order failed or canceled
        if (newOrderStatus == OrderStatus.Failed || newOrderStatus == OrderStatus.Canceled)
        {
            var productsOrderdQuntity = order.OrderLines.ToDictionary(item => item.ProductId, item => item.Quntity);

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
}