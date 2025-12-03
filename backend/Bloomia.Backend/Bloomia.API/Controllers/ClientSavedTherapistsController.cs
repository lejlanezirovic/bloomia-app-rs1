using Bloomia.Application.Modules.SavedTherapists.Command.Add;
using Bloomia.Application.Modules.SavedTherapists.Command.Remove;
using Bloomia.Application.Modules.SavedTherapists.Command.RemoveAll;
using Bloomia.Application.Modules.SavedTherapists.Queries.GetById;
using Bloomia.Application.Modules.SavedTherapists.Queries.GetByName;
using Bloomia.Application.Modules.SavedTherapists.Queries.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientSavedTherapistsController(ISender sender) : ControllerBase
    {

        [Authorize(Roles ="CLIENT")]
        [HttpGet]
        public async Task<PageResult<ListSavedTherapistInfoDto>>GetAllSavedTherapistByClient([FromQuery] ListSavedTherapistsQuery request, CancellationToken ct)
        {
            var userClaim= User.FindFirst("id")?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;       
        }
        [Authorize(Roles ="CLIENT")]
        [HttpPost("save-therapist")]
        public async Task<ActionResult<AddTherapistToSavedTherapistsCommandDto>> SaveTherapistToSavedTherapistsList([FromBody]AddTherapistToSavedTherapistsCommand request, CancellationToken ct)
        {
            var userClaim= User.FindFirst("id")?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return result;
        }

        [Authorize(Roles = "CLIENT")]
        [HttpDelete("remove-therapist-by-id/{therapistId:int}")]
        public async Task<ActionResult<string>> RemoveTherapistFromSavedTherapistList(int therapistId, CancellationToken ct)
        {
            var request = new RemoveSavedTherapistCommand();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.TherapistId = therapistId;
            var result=await sender.Send(request, ct);
            return Ok(result);
        }

        [Authorize(Roles = "CLIENT")]
        [HttpDelete("remove-all-saved-therapists")]
        public async  Task<ActionResult<string>> RemoveAllSavedTherapistsForClient(RemoveAllSavedTherapistsCommand request, CancellationToken ct)
        {
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            var result=await sender.Send(request, ct);
            return Ok(result);
        }

        [Authorize(Roles ="CLIENT")]
        [HttpGet("get-saved-therapist-by-Id/{id:int}")]
        public async Task<ActionResult<GetSavedTherapistByIdCommandDto>> GetSavedTherapistById(int id,CancellationToken ct)
        {
            var request = new GetSavedTherapistByIdCommand();

            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.TherapistId = id;

            var result=await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "CLIENT")]
        [HttpGet("search-saved-therapists-by-name")]
        public async Task<ActionResult<List<GetSavedTherapistByNameCommandDto>>> GetSavedTherapistsBySearch (string search, CancellationToken ct)
        {
            var request = new GetSavedTherapistByNameCommand();
            var userClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userClaim.Value);
            request.UserId = userId;
            request.SerachName=search;

            var result = await sender.Send(request, ct);
            return result;
        }
    }
}
