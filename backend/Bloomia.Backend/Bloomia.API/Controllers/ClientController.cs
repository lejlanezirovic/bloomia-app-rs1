using Bloomia.Application.Modules.Client.Queries.GetById;
using Bloomia.Application.Modules.Client.Queries.GetClientProfileById;
using Bloomia.Application.Modules.Client.Queries.List;
using System.Security.Claims;


[Route("api/client")]
[ApiController]
public sealed class ClientController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("get-by-id/{id:int}")]
    public async Task<GetClientByIdQueryDTO> GetById(int id, CancellationToken ct)
    {
        var clientById=new GetClientByIdQuery {
            
            ClientId = id
        };
        var client = await sender.Send(clientById, ct);
        return client;
    }

    [Authorize(Roles ="CLIENT")]
    [HttpGet("my-profile")]
    public async Task<GetClientProfileByIdQueryDTO> GetMyProfile (CancellationToken ct)
    {
        var userId = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
       
        var user = new GetClientProfileByIdQuery
        {
            UserId = int.Parse(userId.Value)
        };
        var client= await sender.Send(user, ct);
        return client;

    }

    [AllowAnonymous]//promjeniti u authorize role admin
    [HttpGet("search-by-fullname")]
    public async Task<PageResult<ListClientsQueryDto>> GetAllClients([FromQuery] ListClientsQuery request, CancellationToken ct)
    {
        var clients=await sender.Send(request, ct);
        return clients;
    }

}

