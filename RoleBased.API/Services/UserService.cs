using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using RoleBased.API.Entities;
using RoleBased.API.Helpers;
using RoleBased.API.Models;

namespace RoleBased.API.Services
{
   

    public class UserService : IUserService
    {
        private readonly UserContext _context;
       

        private readonly AppSettings _appSettings;
        

        public UserService(IOptions<AppSettings> appSettings, UserContext context,IOptions<Settings> settings)
        {
            _context = new UserContext(settings);
            
            _appSettings = appSettings.Value;
        }
       

        public User Authenticate(string username, string password)
        {
            var user = _context.Users.Find(x => x.Username == username && x.Password == password).SingleOrDefault();

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Find(x => x.Username == user.Username).Any())
                throw new AppException("Username \"" + user.Username + "\" is already taken");

           
            _context.Users.InsertOne(user);
            
            
            

            return user;
        }
         

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.Find(u => true).ToList();
            return users.WithoutPasswords();
        }

        public User GetByName(string name)
        {
            
            var user = _context.Users.Find(x=>x.FirstName==name).FirstOrDefault();
            return user.WithoutPassword();
        }

        public void Update(User userParam, string password)
        {
            var user = _context.Users.Find(u=>u.Id==userParam.Id).FirstOrDefault();
            

           

            if (user==null)
            {
                throw new AppException("User not found");
            }
            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Find(x => x.Username == userParam.Username).Any())
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
                
               
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;
          

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;
            
            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.Password = password;
             
            }

            
        
            _context.Users.ReplaceOne(u => u.Id == userParam.Id, user);



        }
        public User GetById(string id)
        {
           var user= _context.Users.Find(u=>u.Id==id).FirstOrDefault();
           return user;
        }

        public void Delete(string id)
        {
            var user = _context.Users.Find(u=>u.Id==id).SingleOrDefault();
            if (user != null)
            {
                _context.Users.FindOneAndDelete(u => u.Id == user.Id);

            }
        }
        public void Delete(User userIn)
        {
            var user = _context.Users.Find(u => u.Id == userIn.Id).SingleOrDefault();
            if (user != null)
            {
                _context.Users.FindOneAndDelete(u => user.Id == userIn.Id);

            }
        }
    }
}
