using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}