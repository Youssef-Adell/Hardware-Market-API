namespace Core.DTOs.SpecificationDTOs;

public class PagedResult<T>
{
    public PagedResult()
    {
    }
    public PagedResult(IList<T> data, int currentPage, int pageSize, int totalItems)
    {
        Data = data;
        Metadata = new PageMetadata{
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public IList<T> Data { get; init; }
    public PageMetadata Metadata { get; set; }
}

public class PageMetadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
