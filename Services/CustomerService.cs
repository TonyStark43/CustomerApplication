using CustomerApplication.Mapper;
using CustomerApplication.Models;
using CustomerApplication.Repository;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly AddressService _addressService;
        public CustomerService(ICustomerRepository customerRepository, AddressService addressService) 
        {
            _customerRepository = customerRepository;
            _addressService = addressService;
        }

        public async Task<(int, List<CustomerViewModel>)> GetCustomerList( string filterName, int pageNum, int pageSize, string sortBy)
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            int count = 0;
            if(pageNum < 1)
                pageNum = 1;
            if(pageSize < 1)
                pageSize = 10;
            int skip = (pageNum - 1) *pageSize;

            (count, var customerInfoObject) =  await _customerRepository.GetCustomerList(filterName, skip, pageSize, sortBy);
            if (customerInfoObject != null)
            {
                foreach (var item in customerInfoObject)
                {
                    var districtState = await _addressService.GetAddressByDistrictId(item.DistrictId);
                    var customerViewModel = CustomerMapper.MapeCustomerEntityToViewModel(item, districtState);
                    customerList.Add(customerViewModel);
                }
            }
            return (count, customerList);
        }


        public async Task<List<GenderMaster>> GetGenderMasterData()
        {
            return await _customerRepository.GetGenderMasterData();
        }

        public async Task<(bool, string)> SaveCustomer(CustomerEditModel customerEditModel)
        {
            if (customerEditModel == null)
                return (false, "Customer model is null");
            var customerInfo = CustomerMapper.MapCustomerEditToEntityModel(customerEditModel);
            return await _customerRepository.SaveCustomer(customerInfo);
        }


        public async Task<CustomerEditModel> GetCustomerByIdForEdit(int customerId)
        {
            CustomerEditModel customerEditModel = new CustomerEditModel();
            if(customerId > 0)
            {
                var customerInfoModel = await _customerRepository.GetCustomerById(customerId);
                if(customerInfoModel != null)
                {
                    DistrictStateViewModel districtMaster = new DistrictStateViewModel();
                    districtMaster = await _addressService.GetAddressByDistrictId(customerInfoModel.DistrictId);
                    customerEditModel = CustomerMapper.MapCustomerEntityToEditModel(customerInfoModel, districtMaster);
                }
            }
            return customerEditModel;
        }

        public async Task<int> GetTotalCustomerCount()
        {
            return await _customerRepository.GetTotalCustomerCount();
        }

        public async Task<CustomerViewModel> GetCustomerByIdForView(int customerId)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            if (customerId > 0)
            {
                var customerInfoModel = await _customerRepository.GetCustomerById(customerId);
                if (customerInfoModel != null)
                {
                    DistrictStateViewModel districtMaster = new DistrictStateViewModel();
                    districtMaster = await _addressService.GetAddressByDistrictId(customerInfoModel.DistrictId);
                    customerViewModel = CustomerMapper.MapeCustomerEntityToViewModel(customerInfoModel, districtMaster);
                }
            }
            return customerViewModel;
        }

        public async Task<CustomerDetailViewModel> GetCustomerByIdForDetailView(int customerId)
        {
            CustomerDetailViewModel customerDetailViewModel = new CustomerDetailViewModel();
            if (customerId > 0)
            {
                var customerInfoModel = await _customerRepository.GetCustomerById(customerId);
                if (customerInfoModel != null)
                {
                    DistrictStateViewModel districtMaster = new DistrictStateViewModel();
                    districtMaster = await _addressService.GetAddressByDistrictId(customerInfoModel.DistrictId);
                    customerDetailViewModel = CustomerMapper.MapeCustomerEntityToDetailViewModel(customerInfoModel, districtMaster);
                }
            }
            return customerDetailViewModel;
        }

        public async Task<(bool, string)> UpdateCustomer(CustomerEditModel customerEditModel)
        {
            if (customerEditModel != null && customerEditModel.Id > 0)
            {
                var customer = CustomerMapper.MapCustomerEditToEntityModel(customerEditModel);
                return await _customerRepository.UpdateCustomer(customer);
            }
            return (false, "Updation failed");
        }

        public async Task<(bool, string )> DeleteCustomer(int customerId)
        {
            if (customerId > 0)
            {
                return await _customerRepository.DeleteCustomer(customerId);
            }
            return (false, "Deletion failed");
        }
    }
}
