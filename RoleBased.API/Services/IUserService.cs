using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoleBased.API.Entities;

namespace RoleBased.API.Services
{
    public interface IUserService
    {

        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetByName(string name);

        User Create(User user, string password);

        void Update(User userParam, string password);

        User GetById(string id);
        void Delete(string id);
        void Delete(User user);
    }
}
