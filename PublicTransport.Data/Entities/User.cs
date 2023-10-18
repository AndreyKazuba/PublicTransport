#nullable disable
using PublicTransport.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(EntityConstants.NameMaxLength)]
        [MinLength(EntityConstants.NameMinLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(EntityConstants.NameMaxLength)]
        [MinLength(EntityConstants.NameMinLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EntityConstants.PassportNumberMaxLength)]
        public string PassportNumber { get; set; }

        [Required]
        [MaxLength(EntityConstants.PassportIdMaxLength)]
        public string PasspostId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MinLength(0)]
        public int Balance { get; set; }
    }
}
