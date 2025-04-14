namespace API.Controllers.Common;

using Domain.Interfaces.Extensions;
using Infrastructure.Authentication;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[ApiBase(Order = 1)]
public class BuggyController(IApiLocalizer localizer) : ApiBaseController
{
	[HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ProblemDetails { Title = localizer.Translate("_BadRequest") });
    }

    [HttpGet("unauthorised")]
    public ActionResult GetUnauthorised()
    {
        return Unauthorized();
    }

    [HttpGet("validation-error")]
    public ActionResult GetValidationError()
    {
        ModelState.AddModelError("Problem1", "first error");
        ModelState.AddModelError("Problem2", "second error");
        return ValidationProblem();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        throw new Exception("Server error");
    }
}
