using System;
using System.Data.SqlTypes;
using System.Security;
using ToDo.DAL;
using ToDo.Models;

namespace ToDo.Repository;
public class UserRepo : IUserRepo
{
    private readonly ConnectionDatabase _connectionDb;
    public UserRepo()
    {
        _connectionDb = new ConnectionDatabase();
        
    }
    public void AddUser(User user) 
    { 
        _connectionDb.AddUser(user);
    }
    public User? GetUserById(int id)
    {
        return _connectionDb.GetUserById(id);
    }
   
    public void UpdateUser(User user) 
    { 
        
    }
    public void DeleteUser(int id) 
    {
        _connectionDb.DeleteUser(id);
    }
    public User LoginUser(string username, string password)
    {
        return _connectionDb.Login(username, password.ToString());
    }

}
