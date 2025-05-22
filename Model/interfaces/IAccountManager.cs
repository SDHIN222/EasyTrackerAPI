using Microsoft.AspNetCore.Identity;

public interface IAccountManager
{
    Task<IdentityResult> RegisterAccountAsync(UserRegistrationDto model);
    Task<bool> VerifyAccountAsync(string email, string password);
    Task<IdentityUser> GetAccountAsync(string email);
    Task<IEnumerable<IdentityUser>> GetAccountsAsync();
    Task<IdentityResult> UpdateAccountAsync(string email, UserUpdateDto updateData);
    Task<IdentityResult> DeleteAccountAsync(string email);
}