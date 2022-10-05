using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using ToDo.Models;
using Microsoft.Extensions.Configuration;

namespace ToDo.DAL
{
    public class ConnectionDatabase
    {
        private readonly IConfiguration _configuration;
        readonly private string _connectionString;
        private readonly SqlConnection _sqlConnection;
        public ConnectionDatabase()
        {
            _configuration = Configuration();
            _connectionString = _configuration.GetConnectionString("LocalConnection");
            _sqlConnection = new SqlConnection(_connectionString);     
        }
        public IConfiguration Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        /// <summary>
        /// Create SqlCommand for stored procedure
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        private SqlCommand SqlCmd(string sp)
        {
            using (SqlCommand cmd = new SqlCommand(sp))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _sqlConnection;
                return cmd;
            }
        }
        #region Users
        public void AddUser(string firstname, string lastname, string username, string password)
        {
            SqlCommand cmd = SqlCmd("CreateUser");
            cmd.Parameters.AddWithValue("@Firstname", firstname);
            cmd.Parameters.AddWithValue("@Lastname", lastname);
            cmd.Parameters.AddWithValue("@Login", username);
            cmd.Parameters.AddWithValue("@Password", password);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public void UpdateUser(int id, string firstname, string lastname, string username, string password)
        {
            SqlCommand cmd = SqlCmd("UpdateUser");
            cmd.Parameters.AddWithValue("@UserId", id);
            cmd.Parameters.AddWithValue("@Firstname", firstname);
            cmd.Parameters.AddWithValue("@Lastname", lastname);
            cmd.Parameters.AddWithValue("@Login", username);
            cmd.Parameters.AddWithValue("@Password", password);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public void DeleteUser(int id)
        {
            SqlCommand cmd = SqlCmd("DeleteUser");
            cmd.Parameters.AddWithValue("@UserId", id);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public List<User> GetAllUsers()
        {
            SqlCommand cmd = SqlCmd("GetAllUsers");
            try
            {
                _sqlConnection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                List<User> users = new List<User>();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        users.Add(new User(
                            myReader.GetInt32("users_id"),
                            myReader.GetString("users_firstname"),
                            myReader.GetString("users_lastname"),
                            myReader.GetString("users_login"),
                            myReader.GetString("users_password")
                            ));
                    }
                }
                return users;
            }
            catch (Exception)
            {

                throw;
            }
            finally { _sqlConnection.Close(); }
        }
        public User Login(string username, string password)
        {
            SqlCommand cmd = SqlCmd("UserLogin");
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            try
            {
                _sqlConnection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                User? user = null;
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        user = new User(
                            myReader.GetInt32("users_id"),
                            myReader.GetString("users_firstname"),
                            myReader.GetString("users_lastname"),
                            myReader.GetString("users_login"),
                            myReader.GetString("users_password")
                            );
                    }
                }
                return user;
            }
            catch (Exception)
            {

                throw;
            }
            finally { _sqlConnection.Close(); }

        }
        #endregion
        #region Tasks
        public void CreateTask(string titel, string description, int priorities)
        {
            SqlCommand cmd = SqlCmd("AddTask");
            cmd.Parameters.AddWithValue("@Titel", titel);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@PrioritiesId", priorities);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public void UpdateTask(int id, string titel, string description, int priorities)
        {
            SqlCommand cmd = SqlCmd("UpdateTask");
            cmd.Parameters.AddWithValue("@TaskId", id);
            cmd.Parameters.AddWithValue("@Titel", titel);
            cmd.Parameters.AddWithValue("@Description", description);
            cmd.Parameters.AddWithValue("@PrioritiesId", priorities);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public void DeleteTask(int id)
        {
            SqlCommand cmd = SqlCmd("DeleteTask");
            cmd.Parameters.AddWithValue("@TaskId", id);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { _sqlConnection.Close(); }
        }
        public void CompletTask(int id) 
        {
            SqlCommand cmd = SqlCmd("CompletTask");
            cmd.Parameters.AddWithValue("@TaskId", id);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally { _sqlConnection.Close(); }
        }
        public void AddUserToTask(int userId, int taskId)
        {
            SqlCommand cmd = SqlCmd("AddUserToTask");
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@TaskId", taskId);
            try
            {
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public List<ToDoTask> GetAllTask()
        {
            SqlCommand cmd = SqlCmd("GetAllTask");
            try
            {
                _sqlConnection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                List<ToDoTask> list = new List<ToDoTask>();
                while (myReader.Read())
                {
                    list.Add(new ToDoTask(
                        myReader.GetInt32("task_id"),
                        myReader.GetString("task_titel"),
                        myReader.GetString("task_description"),
                        (ToDoTask.Priority)myReader.GetInt32("priorities_id"),
                        myReader.GetDateTime("task_created")
                        ));
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        public List<ToDoTask> GetAllTaskByUserId(int id)
        {
            SqlCommand cmd = SqlCmd("GetAllTaskById");
            cmd.Parameters.AddWithValue("@UserId", id);
            try
            {
                _sqlConnection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                List<ToDoTask> list = new List<ToDoTask>();
                while (myReader.Read())
                {
                    list.Add(new ToDoTask(
                        myReader.GetInt32("task_id"),
                        myReader.GetString("task_titel"),
                        myReader.GetString("task_description"),
                        (ToDoTask.Priority)myReader.GetInt32("priorities_id"),
                        myReader.GetDateTime("task_created")
                        )) ;
                }
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        #endregion
    }
}
