using System.Security.Claims;
using Bloomia.Application.Modules.Users.Commands.Delete;
using Bloomia.Application.Modules.Users.Commands.UploadProfileImage;
using Bloomia.Application.Modules.Users.Queries.GetById;
using Bloomia.Application.Modules.Users.Queries.List;

namespace Bloomia.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController(ISender sender) : ControllerBase
    {
        [Authorize]
        [HttpPost("upload-profile-image")]
        public async Task<ActionResult<UploadProfileImageCommandDto>> UploadProfileImage([FromForm] UploadProfileImageCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim!.Value);

            request.UserId = userId;

            var result = await sender.Send(request, ct);
            return Ok(result);
        }

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
