using Identity.API.Models.Errors;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NetDevPack.Identity.Jwt;
using NetDevPack.Identity.Jwt.Model;
using NetDevPack.Identity.Model;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Identity.API.Controllers;

public class AccountController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppJwtSettings _appJwtSettings;

    public AccountController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppJwtSettings> appJwtSettings)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _appJwtSettings = appJwtSettings.Value;
    }

    [HttpPost]
    [Route("api/v1/accounts/register")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync(RegisterUser registerUser)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true,
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            return Ok(GenerateJwtToken(user.Email));
        }

        foreach (IdentityError error in result.Errors)
        {
            AddError(error.Description);
        }

        return CustomResponse();
    }

    [HttpPost]
    [Route("api/v1/accounts/login")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorData), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginAsync(LoginUser loginUser)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        SignInResult result = await _signInManager
            .PasswordSignInAsync(loginUser.Email, loginUser.Password, isPersistent: false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Ok(GenerateJwtToken(loginUser.Email));
        }

        if (result.IsLockedOut)
        {
            AddError("This user is blocked");
            return CustomResponse();
        }

        AddError("Incorrect user or password");
        return CustomResponse();
    }

    private UserResponse GenerateJwtToken(string email)
    {
        return new JwtBuilder()
            .WithUserManager(_userManager)
            .WithJwtSettings(_appJwtSettings)
            .WithEmail(email)
            .WithJwtClaims()
            .WithUserClaims()
            .WithUserRoles()
            .BuildUserResponse();
    }
}