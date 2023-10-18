using PublicTransport.Data.Constants.Enumerations;
using System.ComponentModel.DataAnnotations;

#nullable disable
namespace PublicTransport.Data.Entities
{
    public class Transport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public TransportType TransportType { get; set; }
    }
}
