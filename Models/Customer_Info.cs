using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Models
{
    public class Customer_Info
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabets are allowed.")]
        [StringLength(50, ErrorMessage = "Name should be 3-50 characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "GenderId must be between 1 and 3.")]
        public int GenderId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "DistrictId must be between 1 and 100.")]
        public int DistrictId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(GenderId))]
        public GenderMaster Gender { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public DistrictMaster District { get; set; }
    }
}