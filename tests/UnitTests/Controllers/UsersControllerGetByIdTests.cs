using FourTails.Api.Configurations.Mappings;
using FourTails.Api.Controllers;
using FourTails.Core.DomainModels;
using FourTails.Services.Container;
using FourTails.UnitTests.TestHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FourTails.UnitTests.Controllers;

public class UsersControllerGetById
{
    private readonly Mock<ILogger<UsersController>> _logger;
    private readonly Mock<IUserService> _userService;
    private readonly Mock<TokenService> _tokenService;
    private readonly Mock<UserManager<User>> _userManager;
    private readonly UsersController _userController;
    
    public UsersControllerGetById()
    {
        var userProfile = new UserProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userProfile));
        var mapper = new Mapper(configuration);

        _logger = new Mock<ILogger<UsersController>>();
        _tokenService = new Mock<TokenService>();
        _userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _userService = new Mock<IUserService>();
        
        _userController = new UsersController(
            _logger.Object, 
            mapper, 
            _userService.Object, 
            _userManager.Object,
            _tokenService.Object
        );
    }

    #region Success
    [Fact]
    public async Task GetUserById_ShouldPass_WhenValidIdIsPassed()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";

        _userService.Setup(x => x.ReadById(id)).ReturnsAsync(UserHelper.GetUser());

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
    #endregion

    #region Fail
    [Fact]
    public async Task GetUserById_ShouldFail_WhenInvalidIdIsPassed()
    {
        // arrange
        var id = "";

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ShouldFail_WhenUserIsInactive()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";
        var user = UserHelper.GetUser();
        user.IsActive = false;

        _userService.Setup(x => x.ReadById(id)).ReturnsAsync(user);

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_ShouldFail_WhenUserIsNull()
    {
        // arrange
        var id = "9f44cb79-821d-4b42-8550-4ea01569d6b6";

        _userService.Setup(x => x.ReadById(id)).ReturnsAsync((User)null);

        // act
        var result = await _userController.GetUserById(id);

        // assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
    #endregion
}