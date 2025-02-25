using Core.DTOs.QueryParametersDTOs;
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

    public async Task<PagedResult<Order>> GetOrders(OrderQueryParameters queryParams)
    {
        //build a query of filtered orders
        var query = appDbContext.Orders.Include(o => o.Customer)
                            //search (Short Circuit if no value in search)
                            .Where(o => string.IsNullOrEmpty(queryParams.Search) || o.Customer.Email.ToLower() == queryParams.Search.ToLower())
                            //filter (Short circuit if no value)
                            .Where(p => queryParams.Status == null || p.Status == queryParams.Status);


        //sort and paginate the above query then execute it
        var pagedOrdersData = await query
                            .Sort("Id", SortDirection.Descending) //to put the latest created orders at the top
                            .Paginate(queryParams.Page, queryParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd orders to pass it to the pagedResult to make it able to calculate the total pages and return it to the user
        var totalOrdersCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Order>(pagedOrdersData, queryParams.Page, queryParams.PageSize, totalOrdersCount);
    }

    public async Task<PagedResult<Order>> GetCustomerOrders(Guid customerId, PaginationQueryParameters queryParams)
    {
        //build a query of filtered orders
        var query = appDbContext.Orders.Where(o => o.CustomerId == customerId).Include(o => o.Customer);

        //sort and paginate the above query then execute it
        var pagedOrdersData = await query
                            .Sort("Id", SortDirection.Descending) //to put the latest created orders at the top
                            .Paginate(queryParams.Page, queryParams.PageSize)
                            .AsNoTracking() //to enhance the performance
                            .ToListAsync();

        //Get total count of filterd orders to pass it to the pagedResult to make it able to calculate the total pages and return it to the user
        var totalOrdersCount = await query.CountAsync();

        //return page of products with pagination metadata
        return new PagedResult<Order>(pagedOrdersData, queryParams.Page, queryParams.PageSize, totalOrdersCount);
    }

    public async Task<Order?> GetOrder(Guid id)
    {
        var order = await appDbContext.Orders.AsNoTracking()
                                            .Include(o => o.OrderLines)
                                            .Include(o => o.Customer)
                                            .FirstOrDefaultAsync(o => o.Id == id);

        return order;
    }

    public void AddOrder(Order order)
    {
        appDbContext.Orders.Add(order);
    }

    public void UpdateOrder(Order order)
    {
        appDbContext.Orders.Update(order);
    }
}
