using FourTails.Core.DomainModels;
using FourTails.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FourTails.DTOs.Payload.User;
using FourTails.DTOs.Payload.Auth;
using FourTails.Services.Container;
using AutoMapper;

namespace FourTails.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly FTDBContext _context;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public UsersController(
        FTDBContext context, 
        ILogger<UsersController> logger,
        IMapper mapper,
        IUserService userService,
        UserManager<User> userManager,
        TokenService tokenService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _userService = userService ?? throw new ArgumentException(nameof(userService));
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        if (!ModelState.IsValid) 
        {
            BadRequest(ModelState);
        }

        var users = await _userService.ReadAllUsers();

        _logger.LogInformation("executing GetAllUsers()");

        return Ok(users);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateUserDetails")]
    public async Task<ActionResult> UpdateUserDetails([FromForm] UserUpdateDetailsDTO userUpdateDetailsDTO)
    {
        try 
        {
            var id = userUpdateDetailsDTO.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid user id.");
            }

            var user = await _userService.ReadById(id);

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            var updatedUser = _userService.Update(user, userUpdateDetailsDTO);

            var userMap = _mapper.Map<User, UserUpdateDetailsDTO>(updatedUser);

            return Ok(userMap);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return BadRequest(e.Message);
        }
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

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try 
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid user id.");
            }

            var user = await _userService.ReadById(id);

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            await _userService.DeleteUser(user);

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(e.Message);
        }
    }
}
