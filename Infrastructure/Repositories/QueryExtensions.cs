using System.Linq.Dynamic.Core;
using Core.Entities;

namespace Infrastructure.Repositories;

public static class QueryExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
    }

}
