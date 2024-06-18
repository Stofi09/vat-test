using System.ComponentModel.DataAnnotations;

namespace Taxually.TechnicalTest.Models
{
    public class VatRegistrationRequest
    {
        [Required(ErrorMessage = "CompanyName is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(2, ErrorMessage = "Country code must be 2 characters.")]
        public string Country { get; set; }
    }
}
