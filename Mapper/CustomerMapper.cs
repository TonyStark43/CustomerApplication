using CustomerApplication.Models;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerViewModel MapeCustomerEntityToViewModel(Customer_Info customer_Info, DistrictStateViewModel districtState)
        {
            CustomerViewModel customerViewModel = new CustomerViewModel();
            if (customer_Info != null && districtState != null)
            {
                customerViewModel.Id = customer_Info.Id;
                customerViewModel.Name = customer_Info.Name;
                customerViewModel.Gender = customer_Info.Gender.GenderName;
                customerViewModel.State = districtState.StateName;
                customerViewModel.District = districtState.DistrictName;
            }
            return customerViewModel;
        }

        public static CustomerDetailViewModel MapeCustomerEntityToDetailViewModel(Customer_Info customer_Info, DistrictStateViewModel districtState)
        {
            CustomerDetailViewModel customerDetailViewModel = new CustomerDetailViewModel();
            if (customer_Info != null && districtState != null)
            {
                customerDetailViewModel.Id = customer_Info.Id;
                customerDetailViewModel.Name = customer_Info.Name;

                customerDetailViewModel.GenderId = customer_Info.GenderId;
                customerDetailViewModel.Gender = customer_Info.Gender.GenderName;

                customerDetailViewModel.StateId = districtState.StateId;
                customerDetailViewModel.State = districtState.StateName;
                customerDetailViewModel.DistrictId = districtState.DistrictId;
                customerDetailViewModel.District = districtState.DistrictName;
            }
            return customerDetailViewModel;
        }

        public static CustomerEditModel MapCustomerEntityToEditModel(Customer_Info customer_Info, DistrictStateViewModel districtMaster)
        {
            CustomerEditModel model = new CustomerEditModel();
            if (customer_Info != null && districtMaster != null)
            {
                model.Id = customer_Info.Id;
                model.Name = customer_Info.Name;
                model.GenderId = customer_Info.GenderId;
                model.StateId = districtMaster.StateId;
                model.DistrictId = districtMaster.DistrictId;
            }
            return model;
        }

        public static Customer_Info MapCustomerEditToEntityModel(CustomerEditModel customerEdit)
        {
            Customer_Info customerEntity = new Customer_Info();
            if(customerEdit != null)
            {
                customerEntity.Id = customerEdit.Id;
                customerEntity.Name = customerEdit.Name;
                customerEntity.GenderId = customerEdit.GenderId;
                customerEntity.DistrictId= customerEdit.DistrictId;
            }
            return customerEntity;
        }
    }
}
