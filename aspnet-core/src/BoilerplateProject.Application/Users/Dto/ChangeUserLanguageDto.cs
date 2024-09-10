using System.ComponentModel.DataAnnotations;

namespace BoilerplateProject.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}