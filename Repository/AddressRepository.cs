using CustomerApplication.Data;
using Microsoft.EntityFrameworkCore;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ILogger<AddressRepository> _logger;
        private readonly ApplicationDbContext _context;
        private static readonly List<DistrictStateViewModel> districtStateList = new List<DistrictStateViewModel>()
        {
                new DistrictStateViewModel { DistrictId = 1, DistrictName = "Gurgaon", StateName = "Haryana" },
                new DistrictStateViewModel { DistrictId = 2, DistrictName = "Faridabad", StateName = "Haryana" },
                new DistrictStateViewModel { DistrictId = 3, DistrictName = "Hisar", StateName = "Haryana" },
                new DistrictStateViewModel { DistrictId = 4, DistrictName = "Alwar", StateName = "Rajasthan"},
                new DistrictStateViewModel { DistrictId = 5, DistrictName = "Jaipur", StateName = "Rajasthan" },
                new DistrictStateViewModel { DistrictId = 6, DistrictName = "Udaipur", StateName = "Rajasthan" },
                new DistrictStateViewModel { DistrictId = 7, DistrictName = "Chandigarh", StateName = "Punjab" },
                new DistrictStateViewModel { DistrictId = 8, DistrictName = "Amritsar", StateName = "Punjab" }
        };

        private static readonly List<StateMaster> stateMaster = new List<StateMaster>()
        {
            new StateMaster { Id = 1, Name = "Haryana"},
            new StateMaster { Id = 2, Name = "Rajasthan"},
            new StateMaster { Id = 3, Name = "Punjab"}
        };

        private static readonly List<DistrictMaster> districtMaster = new List<DistrictMaster>()
        {
            new DistrictMaster { Id = 1, Name = "Gurgaon", StateId = 1},
            new DistrictMaster { Id = 2, Name = "Faridabad", StateId = 1},
            new DistrictMaster { Id = 3, Name = "Hisar", StateId = 1},
            new DistrictMaster { Id = 4, Name = "Alwar", StateId = 2},
            new DistrictMaster { Id = 5, Name = "Jaipur", StateId = 2},
            new DistrictMaster { Id = 6, Name = "Udaipur", StateId = 2},
            new DistrictMaster { Id = 7, Name = "Chandigarh", StateId = 3},
            new DistrictMaster { Id = 8, Name = "Amritsar", StateId = 3}
        };

        public AddressRepository(ILogger<AddressRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<DistrictStateViewModel> GetAddressByDistrictId(int districtId)
        {
            DistrictStateViewModel districtState = new DistrictStateViewModel();
            try
            {
                //local data
                //await Task.CompletedTask;

                //districtState = (from district in districtMaster
                //              join state in stateMaster on district.StateId equals state.Id
                //              where district.Id == districtId
                //              select new DistrictStateViewModel
                //              {
                //                  DistrictId = district.Id,
                //                  StateId = district.StateId,
                //                  DistrictName = district.Name,
                //                  StateName = state.Name
                //              }).FirstOrDefault();



                //remote data - sql db
                districtState = await (from district in _context.DistrictMaster
                                 join state in _context.StateMaster on district.StateId equals state.Id
                                 where district.Id == districtId
                                 select new DistrictStateViewModel
                                 {
                                     DistrictId = district.Id,
                                     StateId = district.StateId,
                                     DistrictName = district.Name,
                                     StateName = state.Name
                                 }).FirstOrDefaultAsync();

                return districtState;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting district's data for district-Id = {districtId}, {ex}");
                return districtState;
            }
            
        }

        public async Task<List<StateMaster>> GetStatesAsync()
        {
            List<StateMaster> states = new List<StateMaster>();
            try
            {
                //dummy await call
                //await Task.CompletedTask;
                //states = stateMaster.ToList();

                states = await _context.StateMaster.ToListAsync();
                return states;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting state's data, {ex}");
                return states;
            }
            
        }

        public async Task<List<DistrictMaster>> GetDistrictsAsync(int stateId)
        {
            List<DistrictMaster> districts = new List<DistrictMaster>();
            try
            {
                //dummy await call
                //await Task.CompletedTask;
                //districts = districtMaster.Where(x => x.StateId == stateId ).ToList();

                var districtQuery = _context.DistrictMaster.Where(d => d.StateId == stateId);
                districts = await districtQuery.ToListAsync();

                return districts;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while getting state's data for state-Id = {stateId}, {ex}");
                return districts;
            }
            
        }

        //public async Task<DistrictMaster> GetDistrictById(int districtId)
        //{
        //    DistrictMaster district = new DistrictMaster();
        //    {
        //        try
        //        {
        //            await Task.CompletedTask;

        //            district = districtMaster.FirstOrDefault(d => d.Id == districtId);
        //        }
        //        catch(Exception ex)
        //        {
        //            _logger.LogError($"Error occured while getting district's data for district-Id = {districtId}, {ex}");
        //        }
        //        return district;
        //    }
        //}
    }
}
