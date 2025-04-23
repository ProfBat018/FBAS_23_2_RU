namespace ControllerFirst.DTO.Responses;

public class PaginatedResult<T> : Result<IEnumerable<T>>
{
    public PaginatedResult(bool isSuccess, IEnumerable<T>? data, int pageNumber, int pageSize, int totalCount, string? message = null)
        : base(isSuccess, data, message)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }

    public static PaginatedResult<T> Success(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount, string? message = null) 
        => new(true, data, pageNumber, pageSize, totalCount, message);

    public static PaginatedResult<T> Error(string message) 
        => new(false, null, 0, 0, 0, message);
}