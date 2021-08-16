using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoleBased.API.Entities;

namespace RoleBased.API.Services
{
    public interface IRequestService
    {
        List<Request> GetAllRequests();

        void CreateRequest(Request request);
                    
        void Update(Request requestParam);

        List<Request> GetRequestsByUser(string userId);

        Request GetRequestById(string reqId);
            
        void Delete(string reqId);
        void Delete( Request request);

    }
}   
