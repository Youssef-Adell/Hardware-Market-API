namespace Core.DTOs.SpecificationDTOs;

public class SpecificationParameters
{
    private const int MaxPageSize = 10;
    private int _page = 1;
    private int _pageSize = MaxPageSize;

    public int Page
    {
        get
        {
            return _page;
        }
        set
        {
            if (value > 0)
                _page = value;
        }
    }
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value <= MaxPageSize && value > 0) ?  value: MaxPageSize;
        }
    }

}
