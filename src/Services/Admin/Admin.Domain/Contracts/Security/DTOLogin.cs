using System.ComponentModel.DataAnnotations;

namespace Admin.Domain.Contracts.Security;

public class DTOLogin
{
    [Required(ErrorMessage = "messageRequired")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "messageRequired")]
    public string Password { get; set; }
}