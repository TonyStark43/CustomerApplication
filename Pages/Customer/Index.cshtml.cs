using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CustomerApplication.Data;
using CustomerApplication.Models;
using CustomerApplication.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using CustomerApplication.Validators;
using Microsoft.AspNetCore.Authorization;

namespace CustomerApplication.Pages.Customer
{
    
    [Authorize]
    public class IndexModel : PageModel
    {
        
        private readonly ApplicationDbContext _context;

        private readonly CustomerService _customerService;
        private readonly AddressService _addressService;
        private readonly ILogger<IndexModel> _logger;
        private readonly CustomerEditModelValidator _validator;

        [BindProperty]
        public CustomerEditModel CustomerCreateModel { get; set; }

        [BindProperty]
        public CustomerEditModel CustomerUpdateModel { get; set; }



        public IndexModel(CustomerService customerService, ILogger<IndexModel> logger, AddressService addressService, CustomerEditModelValidator validator)
        {
            _customerService = customerService;
            _logger = logger;
            _addressService = addressService;
            _validator = validator;
        }


        public async Task<IActionResult> OnGetCustomerData(int id)
        {
            CustomerEditModel customer = new CustomerEditModel();
            if (id > 0)
            {
                customer = await _customerService.GetCustomerByIdForEdit(id);

                if (customer == null)
                {
                    return NotFound();
                }
            }

            var model = new CustomerEditModel
            {
                Id = customer.Id,
                Name = customer.Name,
                GenderId = customer.GenderId,
                StateId = customer.StateId,
                DistrictId = customer.DistrictId
            };

            // Get dropdown values for gender, state, and district
            var genderMaster = await _customerService.GetGenderMasterData();
            var stateMaster = await _addressService.GetStatesAsync();
            var districtsMaster = await _addressService.GetDistrictsAsync(customer.StateId);

            return new JsonResult(new
            {
                customer = model,
                genders = genderMaster,
                states = stateMaster,
                districts = districtsMaster
            });
        }


        public async Task<IActionResult> OnGetTotalCustomerCount()
        {
            return new JsonResult(new { count = await _customerService.GetTotalCustomerCount() });
        }

        public async Task<IActionResult> OnGetDistrictsByState(string stateId)
        {
            var districts = await _addressService.GetDistrictsAsync(int.Parse(stateId));
            return new JsonResult(districts);
        }


        public async Task<IActionResult> OnGetLoadCustomerList(string filterName, int pageNum, int pageSize, string sortBy)
        {
            (var count, var customers) = await _customerService.GetCustomerList(filterName, pageNum, pageSize, sortBy);

            return Partial("_CustomerList", customers);
        }


        public async Task<IActionResult> OnGetGetCustomerDetails(int id)
        {
            CustomerDetailViewModel customer = new CustomerDetailViewModel();
            if (id > 0)
            {
                customer = await _customerService.GetCustomerByIdForDetailView(id);

                if (customer == null)
                {
                    return NotFound();
                }
            }

                // Return the partial view with the customer details
                return Partial("_CustomerDetailsPartial", customer);
        }

        public async Task<IActionResult> OnGetDropdownData()
        {
            var states = await _addressService.GetStatesAsync();  // Fetch state data
            var genders = await _customerService.GetGenderMasterData();  // Fetch gender data

            var data = new
            {
                states = states.Select(s => new { id = s.Id, name = s.Name }).ToList(),
                genders = genders.Select(g => new { id = g.Id, name = g.GenderName }).ToList()
            };

            return new JsonResult(data);
        }
    }
}
