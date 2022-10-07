using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using ToDo.Models;
using Microsoft.Extensions.Configuration;
using System;

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
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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
        /// <summary>
        /// Add user to db
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            SqlCommand cmd = SqlCmd("CreateUser");
            cmd.Parameters.AddWithValue("@Firstname", user.FirstName);
            cmd.Parameters.AddWithValue("@Lastname", user.LastName);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Password", user.GetPassword());
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
        /// <summary>
        /// Update user in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
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
        /// <summary>
        /// Set user_deleted to 1 in db
        /// </summary>
        /// <param name="id"></param>
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
        /// <summary>
        /// Get all users from Users in db
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Find specfic user from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id)
        {
            SqlCommand cmd = SqlCmd("GetUserById");
            cmd.Parameters.AddWithValue("@UserId", id);
            try
            {
                _sqlConnection.Open();
                SqlDataReader myReader = cmd.ExecuteReader();
                User user;
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        user = new(id,
                            myReader.GetString("users_firstname"),
                            myReader.GetString("users_lastname"),
                            myReader.GetString("users_login"),
                            myReader.GetString("users_password")); 
                        return user;
                    }
                    
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
            finally { _sqlConnection.Close(); }
        }
        /// <summary>
        /// Login for user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>user object with all values</returns>
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
                            username,
                            password
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
        /// <summary>
        /// Add task to db in Tasks
        /// </summary>
        /// <param name="task"></param>
        public void CreateTask(ToDoTask task)
        {
            SqlCommand cmd = SqlCmd("AddTask");
            cmd.Parameters.AddWithValue("@Id", task.GUID);
            cmd.Parameters.AddWithValue("@Titel", task.Title);
            cmd.Parameters.AddWithValue("@Description", task.Description);
            cmd.Parameters.AddWithValue("@PrioritiesId", task.TaskPriority);
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
        /// <summary>
        /// Update task in db on Tasks
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(ToDoTask task)
        {
            SqlCommand cmd = SqlCmd("UpdateTask");
            cmd.Parameters.AddWithValue("@TaskId", task.GUID);
            cmd.Parameters.AddWithValue("@Titel", task.Title);
            cmd.Parameters.AddWithValue("@Description", task.Description);
            cmd.Parameters.AddWithValue("@PrioritiesId", task.TaskPriority);
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
        /// <summary>
        /// Set task_deleted to 1 in db
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteTask(string guid)
        {
            SqlCommand cmd = SqlCmd("DeleteTask");
            cmd.Parameters.AddWithValue("@TaskId", guid);
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
        /// <summary>
        /// Set task_completed to 1 in db
        /// </summary>
        /// <param name="guid"></param>
        public void CompletTask(string guid) 
        {
            SqlCommand cmd = SqlCmd("CompletTask");
            cmd.Parameters.AddWithValue("@TaskId", guid);
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
        public void DeleteAllCompletedTasks()
        {
            SqlCommand cmd = SqlCmd("DeleteAllCompletedTasks");
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
        /// <summary>
        /// Add userid and taskid to joiner tabler UsersTask
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId">GUID</param>
        public void AddUserToTask(int userId, string taskId)
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
        /// <summary>
        /// Get all task from db, used on index site
        /// </summary>
        /// <returns></returns>
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
                    Guid.TryParse(myReader.GetString("task_id"), out Guid guid);
                    list.Add(new ToDoTask
                    {
                        Title = myReader.GetString("task_title"),
                        Description = myReader.GetString("task_description"),
                        TaskPriority = (ToDoTask.Priority)myReader.GetInt32("priorities_id"),
                        IsCompleted = myReader.GetBoolean("task_completed"),
                        GUID = guid,
                        Created = myReader.GetDateTime("task_created")
                    });
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
        /// <summary>
        /// Get all task from specific user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
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
                    Guid.TryParse(myReader.GetString("task_id"), out Guid guid);
                    list.Add(new ToDoTask { 
                        Title = myReader.GetString("task_title"),
                        Description = myReader.GetString("task_description"),
                        TaskPriority = (ToDoTask.Priority)myReader.GetInt32("priorities_id"),
                        GUID=guid,
                        IsCompleted = myReader.GetBoolean("task_completed"),
                        Created = myReader.GetDateTime("task_created")
                    }) ;
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
