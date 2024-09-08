using CustomerApplication.Data;
using CustomerApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressRepository> _logger;
        private static readonly List<Customer_Info> CustomerList = new()
        {
            new Customer_Info(){ Id = 1, Name = "Arman Khan", GenderId = 1, DistrictId = 1 },
            new Customer_Info(){ Id = 2, Name = "Ruby", GenderId = 2, DistrictId = 4 },
            new Customer_Info(){ Id = 3, Name = "Farhan", GenderId = 1, DistrictId = 7 },
            new Customer_Info(){ Id = 4, Name = "Humaira", GenderId = 2, DistrictId = 2 },
            new Customer_Info(){ Id = 5, Name = "Juhaib", GenderId = 1, DistrictId = 5 },
            new Customer_Info(){ Id = 6, Name = "Ahshan Ali", GenderId = 1, DistrictId = 4 },
            new Customer_Info(){ Id = 7, Name = "Mahira", GenderId = 2, DistrictId = 5 },
            new Customer_Info(){ Id = 8, Name = "Amira", GenderId = 2, DistrictId = 6 },
            new Customer_Info(){ Id = 9, Name = "Gauri Sawant", GenderId = 3, DistrictId = 1 }
        };

        private static readonly List<GenderMaster> GenderMasters = new()
        {
            new GenderMaster {Id = 1, GenderName = "Male"},
            new GenderMaster {Id = 2, GenderName = "Female"},
            new GenderMaster {Id = 3, GenderName = "Other"}
        };

        public CustomerRepository(ILogger<AddressRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<(int, List<Customer_Info>)> GetCustomerList(string filterName, int skip, int take, string sortBy)
        {
            List<Customer_Info> customers = new List<Customer_Info>();
            int count = 0;
            try
            {


                //local data
                await Task.CompletedTask;
                //count = CustomerList.Count;
                //var query = CustomerList.AsQueryable();


                //remote data - sql db
                var query = _context.CustomerInfos.AsQueryable();
                //count = await  _context.CustomerInfos.CountAsync();
                

                // Apply filters based on the customerName
                if (!string.IsNullOrWhiteSpace(filterName))
                {
                    query = query.Where(c => c.Name.Contains(filterName));
                }


                //sort by
                switch (sortBy.ToLower())
                {
                    case "id_asc":
                        query = query.OrderBy(c => c.Id);
                        break;
                    case "name_asc":
                        query = query.OrderBy(c => c.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(c => c.Name);
                        break;
                    default:
                        query = query.OrderBy(c => c.Id); // Default sorting by Id ascending
                        break;
                }

                // Pagination
                customers =  query
                    .Include(c => c.Gender) //comment when using local data
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                //local data - gender mapping
                //customers.ForEach(c => c.Gender = GenderMasters.FirstOrDefault(g => g.Id == c.GenderId));

                return (count, customers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting customer's data, {ex}");
                return (count, customers);
            }
            
        }


        public async Task<int> GetTotalCustomerCount()
        {
            int count = 0;
            try
            {
                count = await _context.CustomerInfos.CountAsync();

                return (count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting customer count, {ex}");
                return (count);
            }

        }
        public async Task<List<GenderMaster>> GetGenderMasterData()
        {
            List<GenderMaster> genderMaster = new List<GenderMaster>();
            try
            {
                //local data
                //await Task.CompletedTask;
                //genderMaster = GenderMasters;

                //remote data - sql db
                genderMaster = await _context.GenderMaster.ToListAsync();

                return genderMaster;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting gender's data, {ex}");
                return (genderMaster);
            }

        }

        public async Task<(bool, string)> SaveCustomer(Customer_Info customer)
        {
            try
            {
                //await Task.CompletedTask;

                if (customer == null || string.IsNullOrWhiteSpace(customer.Name) || customer.GenderId < 1 || customer.DistrictId < 1)
                {
                    return (false, "Customer is null or required data is missing");
                }

                await _context.CustomerInfos.AddAsync(customer);
                var result = await _context.SaveChangesAsync();

                if(result >  0)
                {
                    return (true, "Customer created successfully");
                }
                return (false, "Customer creation failed - database error");

            }
            catch(Exception ex)
            {
                _logger.LogError($"Error while saving customer data, {ex}");
                return (false, $"Customer creation failed, {ex.Message}");
            }
            
        }

        public async Task<Customer_Info> GetCustomerById(int customerId)
        {
            Customer_Info customer = new Customer_Info();
            try
            {
                
                if(customerId > 0)
                {
                    //local data
                    //await Task.CompletedTask;
                    //var cust = CustomerList.FirstOrDefault(c => c.Id == customerId);

                    //remote data - sql db - deferred execution
                    //var custQuery = _context.CustomerInfos.Where(c => c.Id == customerId);

                    var custQuery = _context.CustomerInfos
                                            .Include(c => c.Gender) // Eagerly load the Gender navigation property
                                            .Where(c => c.Id == customerId);

                    // The query is executed when the result is accessed.
                    var cust = await custQuery.FirstOrDefaultAsync();

                    if (cust != null)
                        customer = cust;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting customer's data for customerId = {customerId} , {ex}");
            }
            return customer;
        }

        public async Task<(bool, string)> UpdateCustomer(Customer_Info customer)
        {
            try
            {
                await Task.CompletedTask;
                if (customer == null || customer.Id < 0 || string.IsNullOrWhiteSpace(customer.Name) || customer.GenderId < 1 || customer.DistrictId < 1)
                {
                    return (false, "Customer is null or required data is missing");
                }

                #region traditional fetch-update-save approach
                //var existingCustomer = await _context.CustomerInfos.FindAsync(customer.Id);

                //if (existingCustomer == null)
                //{
                //    return (false, "Customer not found");
                //}

                //existingCustomer.Name = customer.Name;
                //existingCustomer.GenderId = customer.GenderId;
                //existingCustomer.DistrictId = customer.DistrictId;
                
                //_context.CustomerInfos.Update(existingCustomer);

                //var result = await _context.SaveChangesAsync();
                #endregion

                #region efficient attach-modified-save approach
                _context.CustomerInfos.Attach(customer);
                _context.Entry(customer).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();
                #endregion

                if (result > 0)
                {
                    return (true, "Customer updated successfully");
                }
                return (false, "Customer updation failed - database error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while updating customer data, {ex}");
                return (false, "Customer updation failed");
            }
        }

        public async Task<(bool, string)> DeleteCustomer(int customerId)
        {
            try
            {
                if (customerId <= 0)
                {
                    return (false, "Invalid customer ID");
                }

                var customer = await _context.CustomerInfos.FindAsync(customerId);

                if (customer == null)
                {
                    return (false, "Customer not found");
                }

                _context.CustomerInfos.Remove(customer);

                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    return (true, "Customer deleted successfully");
                }
                return (false, "Customer deletion failed - database error");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while deleting customer data, {ex}");
                return (false, $"Customer deletion failed, {ex.Message}");
            }
        }


    }
}
