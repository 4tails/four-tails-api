using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FourTails.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;

    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(Name = "GetUsers"), Authorize]
    public ActionResult GetUsers()
    {
        _logger.LogInformation("executing GetUsers()");

        return Ok();
    }
}
