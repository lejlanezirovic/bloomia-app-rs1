using Bloomia.Application.Modules.Articles.Commands.Create;
using Bloomia.Application.Modules.Articles.Commands.Delete;
using Bloomia.Application.Modules.Articles.Commands.Update;
using Bloomia.Application.Modules.Articles.Queries.GetById;
using Bloomia.Application.Modules.Articles.Queries.List;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public sealed class ArticlesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<int>> CreateArticle(CreateArticleCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")] 
        public async Task<IActionResult> Update(int id, UpdateArticleCommand command, CancellationToken ct)
        {
            command.Id = id;
            await sender.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteArticleCommand { Id = id }, ct);
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<PageResult<ListArticlesQueryDto>> List([FromQuery] ListArticlesQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<GetArticleByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var article = await sender.Send(new GetArticleByIdQuery { Id = id }, ct);
            return article; // if NotFoundException -> 404 via middleware
        }


    }
}
