using System.ComponentModel.DataAnnotations;

namespace Squares.Api.Contracts;

public sealed class ImportPointListRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    public IReadOnlyCollection<PointDto> Points { get; init; } = Array.Empty<PointDto>();
}

