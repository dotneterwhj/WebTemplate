using Abner.Application.Blog;
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
        var response = _mediator.Send(new BlogCreateCommand(), cancellationToken);

        return Ok(response);
    }
}