using System.Linq.Dynamic.Core;
using Core.Entities;

namespace Infrastructure.Repositories;

public static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : EntityBase
    {
        return query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
    }

    public static IQueryable<T> Sort<T>(this IQueryable<T> query, string? property, string? direction) where T : EntityBase
    {
        if (!string.IsNullOrEmpty(direction) && direction.StartsWith("desc", StringComparison.InvariantCultureIgnoreCase))
            direction = "desc";
        else
            direction = "asc";

        if (!string.IsNullOrEmpty(property))
        {
            return query.OrderBy($"{property} {direction}");
        }

        return query.OrderBy($"Id {direction}"); // now we ensure that all types T is entity and has property called Id thanks to EntityBase
    }

}
