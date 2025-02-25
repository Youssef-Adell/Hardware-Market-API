using System.Linq.Dynamic.Core;
using Core.DTOs.QueryParametersDTOs;

namespace Infrastructure.Repositories;

public static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query, string? property, SortDirection? direction)
    {
        if (!string.IsNullOrEmpty(property))
        {
            try
            {
                if (direction is null)
                    direction = SortDirection.Ascending;

                return query.OrderBy($"{property} {direction}");
            }
            catch
            {
                //if the user enters invalid property to sort with, the excption will be thrown but i will suprress it using this catch and reuturn the query without sorting
            }
        }

        return query;
    }

}
