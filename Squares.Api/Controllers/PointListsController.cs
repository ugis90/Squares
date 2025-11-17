using Microsoft.AspNetCore.Mvc;
using Squares.Api.Contracts;
using Squares.Api.Mappings;
using Squares.Domain.Abstractions;
using Squares.Domain.Entities;

namespace Squares.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PointListsController(
    IPointListRepository repository,
    ISquareDetectionService squareDetectionService)
    : ControllerBase
{
    private const string GetPointListRouteName = "GetPointListById";

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PointListResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var lists = await repository.GetAllAsync(cancellationToken);
        var response = lists.Select(list => list.ToResponse());
        return Ok(response);
    }

    [HttpGet("{id:guid}", Name = GetPointListRouteName)]
    public async Task<ActionResult<PointListResponse>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var list = await repository.GetAsync(id, cancellationToken);
        if (list is null)
        {
            return NotFound();
        }

        return Ok(list.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<PointListResponse>> ImportAsync(ImportPointListRequest request, CancellationToken cancellationToken)
    {
        var pointList = request.ToDomain();
        await repository.AddAsync(pointList, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return CreatedAtRoute(GetPointListRouteName, new { id = pointList.Id }, pointList.ToResponse());
    }

    [HttpPost("{id:guid}/points")]
    public async Task<ActionResult<PointWithIdDto>> AddPointAsync(Guid id, AddPointRequest request, CancellationToken cancellationToken)
    {
        var point = new Point(request.X, request.Y);
        var addedPoint = await repository.AddPointAsync(id, point, cancellationToken);

        if (addedPoint is null)
        {
            return NotFound();
        }

        await repository.SaveChangesAsync(cancellationToken);

        return CreatedAtRoute(GetPointListRouteName, new { id }, addedPoint.ToDto());
    }

    [HttpDelete("{id:guid}/points/{pointId:guid}")]
    public async Task<IActionResult> DeletePointAsync(Guid id, Guid pointId, CancellationToken cancellationToken)
    {
        var removed = await repository.DeletePointAsync(id, pointId, cancellationToken);
        if (!removed)
        {
            return NotFound();
        }

        await repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}/squares")]
    public async Task<ActionResult<SquaresResponse>> GetSquaresAsync(Guid id, [FromQuery] bool countOnly = false, CancellationToken cancellationToken = default)
    {
        var pointList = await repository.GetAsync(id, cancellationToken);

        if (pointList is null)
        {
            return NotFound();
        }

        if (countOnly)
        {
            var count = squareDetectionService.CountSquares(pointList.Points);
            return Ok(new SquaresResponse(count, Array.Empty<SquareDto>()));
        }

        var squares = squareDetectionService
            .GetSquares(pointList.Points)
            .Select(square => square.ToDto())
            .ToArray();

        return Ok(new SquaresResponse(squares.Length, squares));
    }
}

