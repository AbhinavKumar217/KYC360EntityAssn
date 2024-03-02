using System;
namespace KYC360Assn.Models.Request
{
	public class NewAddressesRequest
	{
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}

