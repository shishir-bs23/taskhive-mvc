using Microsoft.AspNetCore.Identity;
namespace TaskHive.Models;

public class Users : IdentityUser
{

    public required string FullName { get; set; }
}