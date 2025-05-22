using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EasyTrackerAPI.Models;  
using EasyTrackerAPI.Data;
public class AccountManager : IAccountManager
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AccountContext _context;
    private readonly ILogger<AccountManager> _logger;

    public AccountManager(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        AccountContext context,
        ILogger<AccountManager> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

   
    public async Task<IdentityResult> RegisterAccountAsync(UserRegistrationDto model)
    {
        try
        {
            
            if (!IsValidEmail(model.Email))
            {
                throw new ArgumentException("Invalid email format");
            }

            
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                
                await _userManager.AddToRoleAsync(user, "User");

                
                await SaveAdditionalUserData(user.Id, model);

                _logger.LogInformation($"User {model.Email} registered successfully");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed");
            throw;
        }
    }

    
    public async Task<SignInResult> LoginAsync(string email, string password)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation($"User {email} logged in");
            }
            else
            {
                _logger.LogWarning($"Failed login attempt for {email}");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
            throw;
        }
    }

    
    public async Task<IdentityUser> GetAccountAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    
    public async Task<IEnumerable<IdentityUser>> GetAccountsAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    
    public async Task<bool> VerifyAccountAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    
    public async Task<IdentityResult> UpdateAccountAsync(string email, UserUpdateDto updateData)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found");

        user.Email = updateData.NewEmail ?? user.Email;
        user.UserName = updateData.NewEmail ?? user.UserName;

        if (!string.IsNullOrEmpty(updateData.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, updateData.NewPassword);
        }

        return await _userManager.UpdateAsync(user);
    }

    
    public async Task<IdentityResult> DeleteAccountAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found");

        return await _userManager.DeleteAsync(user);
    }

    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


    private async Task SaveAdditionalUserData(string userId, UserRegistrationDto model)
    {
        try
        {
            var userInfo = new UserAdditionalInfo
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            await _context.UserAdditionalInfos.AddAsync(userInfo);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save additional user data");
            throw;
        }
    }

    public void RegisterAccount(User account)
    {
        throw new NotImplementedException();
    }

    public User GetAccount(string accountName)
    {
        throw new NotImplementedException();
    }

    public List<User> GetAccounts()
    {
        throw new NotImplementedException();
    }

    public bool VerifyAccount(User account)
    {
        throw new NotImplementedException();
    }
}





public class UserUpdateDto
{
    public string NewEmail { get; set; }
    public string NewPassword { get; set; }
}

