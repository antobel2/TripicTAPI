using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Web_API.Models
{
    // Modèles utilisés comme paramètres des actions AccountController.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "Jeton d’accès externe")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [StringLength(35, ErrorMessage = "La chaîne {0} doit comporter au moins {5} caractères.", MinimumLength = 5)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {1} caractères.", MinimumLength = 1)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La chaîne {0} doit comporter au moins {1} caractères.", MinimumLength = 1)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "La chaîne {0} doit comporter au moins {8} caractères.", MinimumLength = 8)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Fournisseur de connexion")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Clé du fournisseur")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }
}
