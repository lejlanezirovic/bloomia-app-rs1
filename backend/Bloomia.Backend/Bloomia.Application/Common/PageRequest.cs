namespace Bloomia.Application.Common;

public sealed class PageRequest
{
    private const int MaxPageSize = 100;
    public int Page { get; init; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value <= 0 ? 10 : value > MaxPageSize ? MaxPageSize : value;
    }
    [JsonIgnore]
    public int SkipCount => (Page - 1) * PageSize;

    /// <summary>Naziv kolone po kojoj se sortira (npr. "title", "rating"). Null = default sortiranje.</summary>
    public string? SortBy { get; init; }

    /// <summary>true = opadajuce (Z-A, najnovije prvo); false = rastuce (A-Z).</summary>
    public bool SortDescending { get; init; } = false;
}
