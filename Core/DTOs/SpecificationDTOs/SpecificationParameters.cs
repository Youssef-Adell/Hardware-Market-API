namespace Core.DTOs.SpecificationDTOs;

public class SpecificationParameters
{
    private const int MaxPageSize = 50;
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = (value > 0) ? value : _page;
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value <= MaxPageSize && value > 0) ? value : MaxPageSize;
    }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
