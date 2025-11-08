using Bloomia.Application.Modules.Journals.Commands;
using Bloomia.Application.Modules.Journals.Queries.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


[Route("api/journals")]
[ApiController]
public class JournalController(ISender sender) : ControllerBase
{
    [Authorize(Roles ="CLIENT")]
    [HttpPost("create-a-journal")]
    public async Task<ActionResult<CreateJournalCommandDto>> CreateJournalForClient([FromBody] CreateJournalCommand request, CancellationToken ct)
    {
        var userIdClaim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdClaim.Value);

        request.UserId=userId;

        var result = await sender.Send(request, ct);
        return result;
    }

    [AllowAnonymous]
    [HttpGet("get-journal-questions")]
    public async Task<ActionResult<ListQuestionsQueryDto>> GetJournalQuestions(CancellationToken ct)
    {
        return await sender.Send(new ListQuestionsQuery(),ct);
    }
}

