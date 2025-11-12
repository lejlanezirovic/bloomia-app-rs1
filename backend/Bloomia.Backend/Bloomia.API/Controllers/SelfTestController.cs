using Bloomia.Application.Modules.SelfTests.Command.CreateSelfTest;
using Bloomia.Application.Modules.SelfTests.Command.DeleteSelfTest;
using Bloomia.Application.Modules.SelfTests.Command.SubmitSelfTest;
using Bloomia.Application.Modules.SelfTests.Command.UpdateSelfTest;
using Bloomia.Application.Modules.SelfTests.Queries.GetById;
using Bloomia.Application.Modules.SelfTests.Queries.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloomia.API.Controllers
{
    [Route("api/SelfTests")]
    [ApiController]
    public class SelfTestController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("get-all-self-tests")]
        public async Task<ActionResult<ListAllSelfTestsQueryDto>> GetAllSelfTests(CancellationToken ct)
        {
            return await sender.Send(new ListSelfTestQuery(), ct);
        }

        [AllowAnonymous]
        [HttpGet("get-self-test-by-id/{id:int}")]
        public async Task<ActionResult<GetSelfTestByIdQueryDto>> GetSelfTestById(int id, CancellationToken ct)
        {
            var selfTestId = new GetSelfTestByIdQuery
            {
                SelfTestId = id
            };
            return await sender.Send(selfTestId, ct);
        }
        [AllowAnonymous]
        [HttpGet("find-by-name")]
        public async Task<ActionResult<ListAllSelfTestQuerySearchDto>> GetAllSelfTestByName([FromQuery] string search, CancellationToken ct)
        {
            var request = new ListSelfTestsQuerySearch
            {
                Search = search
            };
            return await sender.Send(request, ct);
        }
        [Authorize(Roles ="CLIENT")]
        [HttpPost("create-client-self-test")]
        public async Task<ActionResult<SubmitSelfTestCommandDto>> SubmitClientsSelfTest([FromBody]SubmitSelfTestCommand request, CancellationToken ct)
        {
            var userClaim=User.FindFirst("id")?? User.FindFirst(ClaimTypes.NameIdentifier);
            var userID = int.Parse(userClaim.Value);
            request.UserId = userID;
            var result= await sender.Send(request, ct);
            return result;
        }

        [Authorize(Roles ="ADMIN")]
        [HttpPost("create-self-test")]
        public async Task<ActionResult<CreateSelfTestCommandDto>> CreateNewSelfTest([FromBody] CreateSelfTestCommand request, CancellationToken ct)
        {
            var result= await sender.Send(request, ct);
            return result;
        }
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("remove-self-test-by-id/{id:int}")]
        public async Task<ActionResult> DeleteSelfTestById(int id, CancellationToken ct)
        {
            var deleteRequest = new DeleteSelfTestCommand
            {
                SelfTestId = id
            };
            await sender.Send(deleteRequest, ct);
            return Ok("Self test disabled!");
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("update-self-test-by-id/{id:int}")]
        public async Task<ActionResult<UpdateSelfTestCommandDto>> UpdateSelfTestById(int id, [FromBody] UpdateSelfTestCommand request, CancellationToken ct)
        {
            request.SelfTestId = id;
            var result = await sender.Send(request, ct);
            return result;
        }
    }
}
