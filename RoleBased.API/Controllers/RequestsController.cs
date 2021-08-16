using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleBased.API.Entities;
using RoleBased.API.Helpers;
using RoleBased.API.Models;
using RoleBased.API.Services;

namespace RoleBased.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {

        private IRequestService _requestService;
        private IUserService _userService;      
        private IMapper _mapper;

        public RequestsController(IRequestService requestService, IMapper mapper, IUserService userService)
        {
            _requestService = requestService;
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var requests = _requestService.GetAllRequests();
            return Ok(requests);
        }
        [AllowAnonymous]
        [HttpPost("create")]
        public IActionResult CreateRequest([FromBody] RequestModel model)
        {

            var currentUserId = User.Identity.Name;

            

            model.UserId = currentUserId;
            
            // map model to entity
             var request = _mapper.Map<Request>(model);
             request.User = _userService.GetById(currentUserId);
            try
            {
                // create request
                _requestService.CreateRequest(request);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]RequestModel model)
        {
            // map model to entity and set id
            var request = _mapper.Map<Request>(model);
           request.reqId = id;

            try
            {
                // update request 
                _requestService.Update(request);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _requestService.Delete(id);
            return Ok();
        }

        [HttpGet("GetRequestById/{id}")]
        public IActionResult GetRequestById(string id)
        {
            var request = _requestService.GetRequestById(id);
            var model = _mapper.Map<RequestModel>(request);
            return Ok(model);
        }

        [HttpGet("GetRequestsByUser/{id}")]
        public IActionResult GetRequestsByUser(string id)
        {

            var request = _requestService.GetRequestsByUser(id);
            
            return Ok(request);
            
        }

    }
}
