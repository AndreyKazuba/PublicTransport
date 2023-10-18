using PublicTransport.Data.Constants.Enumerations;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace PublicTransport.Data.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public TransportType TransportType { get; set; }

        [Required]
        public string TransportNumber { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
