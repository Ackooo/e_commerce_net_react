using Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace API.Controllers;

public class BuggyController :BaseApiController
{
    private readonly IStringLocalizer<Resource> _localizer;

    public BuggyController(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ProblemDetails{Title = _localizer["_BadRequest"] });
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