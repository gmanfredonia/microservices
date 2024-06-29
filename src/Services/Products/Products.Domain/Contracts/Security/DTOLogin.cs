using System.ComponentModel.DataAnnotations;

namespace Models.Security;

public class DTOLogin
{
    [Required(ErrorMessage = "messageRequired")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "messageRequired")]
    public string Password { get; set; }
}