using System.ComponentModel.DataAnnotations;

namespace KYC360Assn.Models.DBEntities
{
    public class Address
    {
        [Key] public int Id { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public int? EntityId { get; set; }
        public Entity? Entity { get; set; }
    }
}
