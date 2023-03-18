using Microsoft.AspNetCore.Mvc;

namespace FourTails.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    public UsersController()
    {
    }

    [HttpGet(Name = "GetUsers")]
    public ActionResult GetUsers()
    {
        return Ok();
    }
}
