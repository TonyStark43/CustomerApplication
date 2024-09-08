using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerApplication.Models
{
    public class MasterDataModels
    {
        public class StateMaster
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "State name cannot exceed 100 characters.")]
            public string Name { get; set; }

            // Navigation property
            public ICollection<DistrictMaster> Districts { get; set; }
        }


        public class DistrictMaster
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "District name cannot exceed 100 characters.")]
            public string Name { get; set; }

            [Required]
            public int StateId { get; set; }

            // Navigation properties
            [ForeignKey(nameof(StateId))]
            [JsonIgnore]
            public StateMaster State { get; set; }

            [JsonIgnore]
            public ICollection<Customer_Info> Customers { get; set; }
        }


        public class GenderMaster
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "Gender name cannot exceed 50 characters.")]
            public string GenderName { get; set; }

            // Navigation property
            [JsonIgnore]
            public ICollection<Customer_Info> Customers { get; set; }
        }


        public class DistrictStateViewModel
        {
            public int DistrictId { get; set; }
            public int StateId { get; set; }
            public string DistrictName { get; set; }
            public string StateName { get; set; }
        }
    }
}
