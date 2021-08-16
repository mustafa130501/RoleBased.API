using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using RoleBased.API.Entities;
using RoleBased.API.Helpers;
using RoleBased.API.Models;
using RoleBased.API.Services;
using ServiceStack.FluentValidation.Validators;

namespace RoleBased.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger  _logger;
        private IUserService _userService;
        private IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper,ILogger<UsersController> logger)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            
             _logger.LogInformation("Start : Getting item details for {Username}",model.Username);
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
             _logger.LogInformation("Completed : Item details for {ID}", user.Id);
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {

            var users = _userService.GetAll();
            _logger.LogInformation("Start : Getting item details for {Username}",users.ToArray());
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            // only allow admins to access other user records

            var currentUsername = User.Identity.Name;
            _logger.LogInformation("Current {Username}",currentUsername);
            if (!User.IsInRole(Role.Admin))
                return Forbid();

            else
            {
                _logger.LogInformation("Current Username {Username}",User);

            }

            var user = _userService.GetByName(name);

            if (user == null)
                return NotFound();
            else
                _logger.LogInformation("Getting item details for {Firstname}",user.FirstName);

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {

            // map model to entity
            var user = _mapper.Map<User>(model);

            
            _logger.LogInformation("Register User {User}",user);
            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                _logger.LogInformation("AppException {AppException}",ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;
            
            _logger.LogInformation("Getting item details for {Id}",user.Id);

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                _logger.LogInformation("Updated {User}",user.Id);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                _logger.LogInformation("AppException error  {Error}",ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(string id)
        {
            _logger.LogInformation("Id : {Id}",id);

            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);

            _logger.LogInformation("GetbyId {Username}",model.Username);

            return Ok(model);
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _userService.Delete(id);
            _logger.LogInformation("Id User deleted {Id}",id);
            return Ok();
        }

    }
}
