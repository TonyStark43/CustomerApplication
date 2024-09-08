using CustomerApplication.Models;

namespace CustomerApplication.Validators
{
    public class CustomerEditModelValidator
    {
        public List<string> Validate(CustomerEditModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 3)
            {
                errors.Add("Name must be at least 3 characters long.");
            }


            if (string.IsNullOrEmpty(model.Name) || model.Name.Length > 50)
            {
                errors.Add("Name must be at most 50 characters long.");
            }

            if (model.GenderId < 1)
            {
                errors.Add("Please select a valid gender.");
            }

            if (model.DistrictId < 1)
            {
                errors.Add("Please select a valid District.");
            }

            return errors;
        }
    }

}