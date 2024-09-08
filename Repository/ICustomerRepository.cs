using CustomerApplication.Models;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Repository
{
    public interface ICustomerRepository
    {
        Task<(int, List<Customer_Info>)> GetCustomerList(string filterName, int skip, int take, string sortBy);

        Task<List<GenderMaster>> GetGenderMasterData();

        Task<Customer_Info> GetCustomerById(int customerId);
        Task<int> GetTotalCustomerCount();

        Task<(bool, string)> SaveCustomer(Customer_Info customer);
        Task<(bool, string)> UpdateCustomer(Customer_Info customer);
        Task<(bool, string)> DeleteCustomer(int customerId);
    }
}
