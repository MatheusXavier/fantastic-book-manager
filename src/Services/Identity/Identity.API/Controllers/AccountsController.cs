using Identity.API.Models.Errors;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using NetDevPack.Identity.Interfaces;
using NetDevPack.Identity.Jwt.Model;
using NetDevPack.Identity.Model;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Identity.API.Controllers;

public class AccountsController : MainController
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtBuilder _jwtBuilder;

    public AccountsController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IJwtBuilder jwtBuilder)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtBuilder = jwtBuilder;
    }

    [HttpPost("api/v1/accounts/register")]
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
            return Ok(await GenerateJwtToken(user.Email));
        }

        foreach (IdentityError error in result.Errors)
        {
            AddError(error.Description);
        }

        return CustomResponse();
    }

    [HttpPost("api/v1/accounts/login")]
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
            return Ok(await GenerateJwtToken(loginUser.Email));
        }

        if (result.IsLockedOut)
        {
            AddError("This user is blocked");
            return CustomResponse();
        }

        AddError("Incorrect user or password");
        return CustomResponse();
    }

    private async Task<UserResponse> GenerateJwtToken(string email)
    {
        return await _jwtBuilder
            .WithEmail(email)
            .WithJwtClaims()
            .WithUserClaims()
            .WithUserRoles()
            .BuildUserResponse();
    }
}