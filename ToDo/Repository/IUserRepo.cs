using System.Security;
using ToDo.Models;

namespace ToDo.Repository
{
    public interface IUserRepo
    {
        public void AddUser(User user);
        public User? GetUserById(int id);
        public void UpdateUser(User user);
        public void DeleteUser(int id);
        public User LoginUser(string username, string password);
    }
}
