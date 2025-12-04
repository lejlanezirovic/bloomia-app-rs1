using Bloomia.Application.Modules.Users.Commands.Delete;
using Bloomia.Application.Modules.Users.Commands.Update;
using Bloomia.Application.Modules.Users.Queries.GetById;
using Bloomia.Application.Modules.Users.Queries.List;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<PageResult<ListUsersQueryDto>> List([FromQuery] ListUsersQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<GetUserByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var user = await sender.Send(new GetUserByIdQuery { Id = id }, ct);
            return user; // if NotFoundException -> 404 via middleware
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteUserCommand { Id = id }, ct);
            return NoContent();
        }
    }
}
