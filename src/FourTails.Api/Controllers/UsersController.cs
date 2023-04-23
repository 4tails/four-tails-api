using FourTails.Core.DomainModels;
using FourTails.DataAccess;
using FourTails.DTOs.Payload;
using FourTails.DTOs.PayLoad;
using FourTails.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FourTails.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly FTDBContext _context;
    private readonly ILogger _logger;
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public UsersController(
        FTDBContext context, 
        ILogger<UsersController> logger, 
        UserManager<User> userManager,
        TokenService tokenService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
       _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    [HttpGet("GetAll")]
    [Authorize]
    public ActionResult GetAllUsers()
    {
        if (!ModelState.IsValid) 
        {
            BadRequest(ModelState);
        }

        _logger.LogInformation("executing GetAllUsers()");

        return Ok();
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(UserRegistrationDTO request)
    {
        if (!ModelState.IsValid)
        {
            BadRequest(ModelState);
        }

        var response = await _userManager.CreateAsync
        (
            new User 
            {
                Age = request.Age,
                Address = request.Address,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username
            },
            request.Password
        );

        if (response.Succeeded)
        {
            return CreatedAtAction(nameof(Register), new {email = request.Email}, request);
        }
         
        response
            .Errors
            .Select(x => new {
                Code = x.Code, Description = x.Description
            })
            .ToList()
            .ForEach(x => ModelState.AddModelError(x.Code, x.Description));
        
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userManager = await _userManager.FindByEmailAsync(request.Email);

        if (userManager.Equals(null))
        {
            return BadRequest("Bad Credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(userManager, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad Credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(x => x.Email == request.Email);
        if (userInDb is null)
            return Unauthorized();

        var accessToken = _tokenService.CreateToken(userInDb);

        await _context.SaveChangesAsync();

        return Ok(new AuthResponseDTO
        (
            UserName: userInDb.UserName,
            Email: userInDb.Email,
            Token: accessToken
        ));
    }
}
