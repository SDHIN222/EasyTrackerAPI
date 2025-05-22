
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountManager _accountManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountManager accountManager,
        ILogger<AccountController> logger)
    {
        _accountManager = accountManager;
        _logger = logger;
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] LoginDto model)
    {
        try
        {
            var result = await _accountManager.VerifyAccountAsync(model.Email, model.Password);

            _logger.LogInformation($"Verification attempt for {model.Email}: {result}");

            return result ? Ok(new { IsVerified = true })
                         : Unauthorized(new { IsVerified = false });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Verification error");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto model)
    {
        try
        {
            _logger.LogInformation($"Registration attempt for {model.Email}");

            var result = await _accountManager.RegisterAccountAsync(model);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            var verificationResult = await _accountManager.VerifyAccountAsync(model.Email, model.Password);

            var user = await _accountManager.GetAccountAsync(model.Email);

            return Ok(new
            {
                Success = true,
                IsVerified = verificationResult,
                User = new
                {
                    Id = user.Id,
                    user.Email,
                    user.UserName
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed");
            return BadRequest(new
            {
                Success = false,
                Error = ex.Message
            });
        }
    }

    [HttpGet("get/{email}")]
    public async Task<IActionResult> Get(string email)
    {
        try
        {
            var user = await _accountManager.GetAccountAsync(email);

            if (user == null)
            {
                _logger.LogWarning($"User {email} not found");
                return NotFound();
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user {email}");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var users = await _accountManager.GetAccountsAsync();

            _logger.LogInformation($"Retrieved {users.Count()} users");

            return Ok(users.Select(u => new
            {
                u.Id,
                u.Email,
                u.UserName
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}

