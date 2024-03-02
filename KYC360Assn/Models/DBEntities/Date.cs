using System.ComponentModel.DataAnnotations;

namespace KYC360Assn.Models.DBEntities
{
    public class Date
    {
        [Key] public int Id { get; set; }
        public string? DateType { get; set; }
        public DateTime? DateValue { get; set; }

        public int? EntityId { get; set; }
        public Entity? Entity { get; set; }
    }
}
