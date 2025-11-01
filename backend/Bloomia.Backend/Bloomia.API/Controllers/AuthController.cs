using Bloomia.Application.Modules.Auth.Commands.Login;
using Bloomia.Application.Modules.Auth.Commands.Logout;
using Bloomia.Application.Modules.Auth.Commands.Refresh;
using Bloomia.Application.Modules.Auth.Commands.Register;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }
    [HttpPost("register-as-client")]
    [AllowAnonymous]
    public async Task<ActionResult<UserRegisterCommandDto>> ClientRegistration([FromBody] UserRegisterCommand command, CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginCommandDto>> Refresh([FromBody] RefreshTokenCommand command, CancellationToken ct)
    {
        return Ok(await mediator.Send(command, ct));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task Logout([FromBody] LogoutCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
    }
    //svi ostali endpointi koji su budu ticali striktno jednog usera nece biti allow anonymous bit ce onaj katanac na endpointu za JTW token Bearer
}