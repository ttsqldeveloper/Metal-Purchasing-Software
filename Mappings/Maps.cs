using AutoMapper;
using MPA.Data;
using MPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MPA.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Employee, EmployeeVM>().ReverseMap();
            CreateMap<Order, OrderVM>() .ReverseMap();
            // CreateMap<AlloyItem, AlloyItemVM>() .ReverseMap();

            // CreateMap<Order, OrderVM>()
            //     .ForMember(dest => dest.ReceivedMass, opt => opt.MapFromj)
            //     .ReverseMap();

            CreateMap<Supplier, SupplierVM>().ReverseMap();
            CreateMap<Alloy, AlloyVM>().ReverseMap();
            CreateMap<AlloyOrder, AlloyOrderVM>().ReverseMap();
            CreateMap<ReceivedOrder, ReceivedOrderVM>().ReverseMap();
            CreateMap<ReceivedAlloyOrder, ReceivedAlloyOrderVM>().ReverseMap();
            CreateMap<SupplierEmail, EmailItemVM>().ReverseMap();
        }
    }
}
