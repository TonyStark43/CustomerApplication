using CustomerApplication.Repository;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Services
{
    public class AddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }



        public async Task<DistrictStateViewModel> GetAddressByDistrictId(int districtId)
        {
            return await _addressRepository.GetAddressByDistrictId(districtId);
        }
        public async Task<List<StateMaster>> GetStatesAsync()
        {
            return await _addressRepository.GetStatesAsync();
        }
        public async Task<List<DistrictMaster>> GetDistrictsAsync(int stateId)
        {
            return await _addressRepository.GetDistrictsAsync(stateId);
        }
    }
}
