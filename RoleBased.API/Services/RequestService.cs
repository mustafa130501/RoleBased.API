using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RoleBased.API.Entities;
using RoleBased.API.Helpers;
using RoleBased.API.Models;

namespace RoleBased.API.Services
{
    public class RequestService:IRequestService
    {
        private readonly UserContext _context;
        private readonly AppSettings _appSettings;

        public RequestService(IOptions<AppSettings> appSettings, UserContext context,IOptions<Settings> settings)
        {   
            _context = new UserContext(settings);
            
            _appSettings = appSettings.Value;
        }

        public List<Request> GetAllRequests()
        {
            var requests = _context.Requests.Find(r => true).ToList();

            return requests;
        }

        public void CreateRequest(Request request)
        {

            try
            {
                _context.Requests.InsertOne(request);
            }
            catch (Exception)
            {
                throw  new AppException("Request is not created");
            }
            
        }

        public void Update(Request requestParam)
        {
            var request = _context.Requests.Find(r => r.reqId == requestParam.reqId).FirstOrDefault();

            if (request==null)
            {
                throw new AppException("Request not Found");
            }

            //_context.Requests.ReplaceOne(r=>r.reqId==requestParam.reqId,request);
            _context.Requests.FindOneAndReplace(r=>r.reqId==requestParam.reqId,request);

        }

        public List<Request> GetRequestsByUser(string _Id)
        {
            var requests = _context.Requests.Find(r=>r.UserId==_Id).ToList();
             
            
            return requests;
                
            
        }
        public List<Request> GetRequestsByUser(User _user)
        {           
            var requests = _context.Requests.Find(r => r.UserId==_user.Id).ToList();
            
            return requests;
                
            
        }

        public Request GetRequestById(string reqId)
        {
            var request = _context.Requests.Find(r => r.reqId == reqId).FirstOrDefault();

            return request;
        }

        public void Delete(string reqId)
        {
            var request = _context.Requests.Find(r => r.reqId == reqId).SingleOrDefault();
            if (request!=null)
            {
                _context.Requests.FindOneAndDelete(r => r.reqId == request.reqId);
            }
        }

        public void Delete(Request request)
        {
            var request1 = _context.Requests.Find(r => r.reqId == request.reqId).SingleOrDefault();
            if (request1!=null)
            {
                _context.Requests.FindOneAndDelete(r => r.reqId == request1.reqId);
            }

        }
    }
}
