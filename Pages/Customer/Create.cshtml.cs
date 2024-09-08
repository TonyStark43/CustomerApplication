using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CustomerApplication.Data;
using CustomerApplication.Models;
using static CustomerApplication.Models.MasterDataModels;
using CustomerApplication.Repository;
using CustomerApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CustomerApplication.Validators;

namespace CustomerApplication.Pages.Customer
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        private readonly CustomerService _customerService;
        private readonly CustomerEditModelValidator _validator;

        public UpdateModel(CustomerService customerService, CustomerEditModelValidator validator)
        {
            _customerService = customerService;
            _validator = validator;
        }

        public IActionResult OnGet()
        {
            // Redirect to the Index page
            return RedirectToPage("/Customer/Index");
        }





        // update customer
        public async Task<IActionResult> OnPostUpdateCustomer(CustomerEditModel CustomerUpdateModel)
        {
            bool resSuccess = false;
            string resultMessage = string.Empty;

            var errors = _validator.Validate(CustomerUpdateModel);

            if (errors != null && errors.Any())
            {
                return new JsonResult(new { success = resSuccess, message = errors.FirstOrDefault() });
            }
            else
            {
                if(!ModelState.IsValid)
                {
                    resultMessage = ModelState.Values
                                               .SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .FirstOrDefault();
                }
                else
                {
                    (resSuccess, resultMessage) = await _customerService.UpdateCustomer(CustomerUpdateModel);
                }


                return new JsonResult(new { success = resSuccess, message = resultMessage });
            }

        }


        // delete customer
        public async Task<IActionResult> OnPostDeleteCustomer(int customerId)
        {
            if (customerId > 0)
            {
                (var result, var resMessage) = await _customerService.DeleteCustomer(customerId);

                return new JsonResult(new { success = result, message = resMessage });
            }
            return new JsonResult(new { success = false, message = "Customer not found" });
        }


        // create customer
        public async Task<IActionResult> OnPostCreateCustomer(CustomerEditModel CustomerCreateModel)
        {
            bool resSuccess = false;
            string resultMessage = string.Empty;

            var errors = _validator.Validate(CustomerCreateModel);

            if (errors != null && errors.Any())
            {
                return new JsonResult(new { success = false, message = errors.FirstOrDefault() });
            }
            else
            {

                if (!ModelState.IsValid)
                {
                    resultMessage = ModelState.Values
                                               .SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .FirstOrDefault();
                }
                else
                {
                    (resSuccess, resultMessage) = await _customerService.SaveCustomer(CustomerCreateModel);
                }

                return new JsonResult(new { success = resSuccess, message = resultMessage });

            }
        }

    }
}
