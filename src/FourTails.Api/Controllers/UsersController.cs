using AutoMapper;
using FourTails.Api.Validations;
using FourTails.Core.DomainModels;
using FourTails.DTOs.Payload.Auth;
using FourTails.DTOs.Payload.User;
using FourTails.Services.Container;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FourTails.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public UsersController(
        ILogger<UsersController> logger,
        IMapper mapper,
        IUserService userService,
        UserManager<User> userManager,
        TokenService tokenService)
    {
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
        try
        {
            if (!ModelState.IsValid) 
            {
                BadRequest(ModelState);
            }

            var users = await _userService.ReadAllUsers();

            _logger.LogInformation("executing GetAllUsers()");

            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        try 
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid user id.");
            }

            var user = await _userService.ReadById(id);

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("UpdateUserDetails")]
    public async Task<ActionResult> UpdateUserDetails([FromForm] UserUpdateDetailsDTO request)
    {
        try 
        {
            var userUpdateDetailsDTOValidation = new UserUpdateDetailsDTOValidation();
            var validationResult = userUpdateDetailsDTOValidation.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var user = await _userService.ReadById(request.Id);

            if (!user.IsActive)
            {
                return NotFound("User is inactive.");
            }

            if (user == null)
            {
                return NotFound("User does not exist.");
            }

            var updatedUser = _userService.Update(user, request);

            var userMap = _mapper.Map<User, UserUpdateDetailsDTO>(updatedUser);

            return Ok(userMap);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromForm] UserRegistrationDTO request)
    {
        try 
        {
            var userRegistrationDTOValidation = new UserRegistrationDTOValidation();
            var validationResult = userRegistrationDTOValidation.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.First());
            }

            var response = await _userManager.CreateAsync
            (
                new User 
                {
                    Age = request.Age,
                    Address = request.Address,
                    CreatedBy = request.Email,
                    CreatedOn = DateTime.UtcNow,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = $"{request.FirstName}_{request.LastName}".ToLower(),
                    IsActive = true
                },
                request.Password
            );

            return CreatedAtAction(nameof(Register), new {email = request.Email}, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromForm] AuthRequestDTO request)
    {
        try {
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

            var userAndAccessToken = _tokenService.CreateToken(request);
            var userToken = userAndAccessToken?.Select(x => x.Key).First();

            if (string.IsNullOrEmpty(userToken))
            {
                return Unauthorized();
            }
            
            var user = userAndAccessToken?.Select(x => x.Value).First();    

            return Ok(user);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
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

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
