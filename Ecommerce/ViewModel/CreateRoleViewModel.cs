using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        public string RoleName { get; set; }
    }
}
