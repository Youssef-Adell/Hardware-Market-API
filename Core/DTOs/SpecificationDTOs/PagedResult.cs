namespace Core.DTOs.SpecificationDTOs;

public class PagedResult<T>
{
    public PagedResult()
    {
    }
    public PagedResult(IReadOnlyCollection<T> data, int currentPage, int pageSize, int totalItems)
    {
        Data = data;
        Metadata = new PageMetadata
        {
            Page = currentPage,
            PageSize = data.Count,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public IReadOnlyCollection<T> Data { get; init; }
    public PageMetadata Metadata { get; set; }
}

public class PageMetadata
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}
