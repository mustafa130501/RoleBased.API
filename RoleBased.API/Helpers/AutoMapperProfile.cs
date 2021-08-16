using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RoleBased.API.Entities;
using RoleBased.API.Models;

namespace RoleBased.API.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<Request, RequestModel>();
            CreateMap<RequestModel, Request>();
            CreateMap<UpdateRequestModel, Request>();
            //CreateMap<Request, UpdateRequestModel>();
            
        }
    }
}
