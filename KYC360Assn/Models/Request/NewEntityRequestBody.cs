using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KYC360Assn.Models.DBEntities;

namespace KYC360Assn.Models.Request
{
	public class NewEntityRequestBody
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public List<NewNamesRequest> Names { get; set; }
        public List<NewAddressesRequest>? Addresses { get; set; }
        public List<NewDateRequest> Dates { get; set; }
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
    }
}

