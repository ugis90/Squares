using System.ComponentModel.DataAnnotations;

namespace Squares.Api.Contracts;

public sealed class AddPointRequest
{
    [Required]
    public int X { get; init; }

    [Required]
    public int Y { get; init; }
}

