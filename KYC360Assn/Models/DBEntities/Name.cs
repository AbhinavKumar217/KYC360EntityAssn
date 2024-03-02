using System.ComponentModel.DataAnnotations;

namespace KYC360Assn.Models.DBEntities
{
    public class Name
    {
        [Key] public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Surname { get; set; }

        public int? EntityId { get; set; }
        public Entity? Entity { get; set; }
    }
}
