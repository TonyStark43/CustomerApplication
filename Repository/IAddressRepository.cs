using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Repository
{
    public interface IAddressRepository
    {
        Task<DistrictStateViewModel> GetAddressByDistrictId(int districtId);
        Task<List<StateMaster>> GetStatesAsync();
        Task<List<DistrictMaster>> GetDistrictsAsync(int stateId);
    }
}
