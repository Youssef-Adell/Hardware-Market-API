using Core.DTOs.SpecificationDTOs;
using Core.Entities.OrderAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly AppDbContext appDbContext;

    public OrdersRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public async Task<PagedResult<Order>> GetOrders(OrdersSpecificationParameters specsParams)
    {
        //build a query of filtered orders
        var query = appDbContext.Orders
                            //search (Short Circuit if no value in search)
                            .Where(p => string.IsNullOrEmpty(specsParams.Search) || p.CustomerEmail.ToLower() == specsParams.Search.ToLower())
                            //filter (Short circuit if no value)
                            .Where(p => specsParams.Status == null || p.Status == specsParams.Status);


        //sort and paginate the above query then execute it
        var pagedOrdersData = await query
                            .Sort(specsParams.SortBy, specsParams.SortDirection)
                            .Paginate(specsParams.Page, specsParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd orders to pass it to the pagedResult to make it able to calculate the total pages and return it to the user
        var totalOrdersCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Order>(pagedOrdersData, specsParams.Page, specsParams.PageSize, totalOrdersCount);
    }

    public async Task<PagedResult<Order>> GetCustomerOrders(string customerEmail, SpecificationParameters specsParams)
    {
        //build a query of filtered orders
        var query = appDbContext.Orders.Where(o => o.CustomerEmail == customerEmail);

        //sort and paginate the above query then execute it
        var pagedOrdersData = await query
                            .Sort(specsParams.SortBy, specsParams.SortDirection)
                            .Paginate(specsParams.Page, specsParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd orders to pass it to the pagedResult to make it able to calculate the total pages and return it to the user
        var totalOrdersCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Order>(pagedOrdersData, specsParams.Page, specsParams.PageSize, totalOrdersCount);
    }

    public async Task<Order?> GetOrder(int id)
    {
        var order = await appDbContext.Orders.AsNoTracking()
                                            .Include(o => o.OrderItems)
                                            .FirstOrDefaultAsync(o => o.Id == id);

        return order;
    }

    public void AddOrder(Order order)
    {
        appDbContext.Orders.Add(order);
    }
}
