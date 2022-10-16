using Abner.Api.Host.SeedWork;
using Abner.Application.BlogApp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Abner.Api.Host.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> QueryBlog([FromQuery] Pager pager, CancellationToken cancellationToken)
    {
        var resposnse = await _mediator.Send(new BlogPageQuery(pager.PageIndex, pager.PageSize), cancellationToken);

        return Ok(resposnse);
    }

    // GET
    [HttpGet("{id}")]
    public async Task<IActionResult> QueryBlog(Guid id, CancellationToken cancellationToken)
    {
        var resposnse = await _mediator.Send(new BlogQuery() { Id = id }, cancellationToken);

        return Ok(resposnse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromBody] BlogCreateRequest request,
        CancellationToken cancellationToken)
    {
        var response =
            await _mediator.Send(new BlogCreateCommand(request.Title, request.Description), cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(Guid id,
        CancellationToken cancellationToken)
    {
        var response =
            await _mediator.Send(new BlogDeleteCommand(id), cancellationToken);

        return Ok(response);
    }
}