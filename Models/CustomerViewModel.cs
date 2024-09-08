using System.ComponentModel.DataAnnotations;

namespace CustomerApplication.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public string Gender{ get; set; }
        public string District{ get; set; }
        public string State{ get; set; }
    }

    
    public class CustomerEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Only alphabets are allowed in name.")]
        [StringLength(50, ErrorMessage = "Name should be 3-50 characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "District is required")]
        public int DistrictId { get; set; }
    }


    public class CustomerDetailViewModel : CustomerViewModel
    {
        public int GenderId { get; set; }
        public int DistrictId { get; set; }
        public int StateId { get; set; }
    }

    public class CustomerListResponse
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }
        public int TotalCount { get; set; }
    }

}
