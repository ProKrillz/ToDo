namespace ToDo.Models
{
    public class User
    {
        private string? _password;
        public int Id { get; init; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password
        {
            set
            {
                _password = value;
            } 
        }
        public User(int id, string firstname, string lastname, string username, string password)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;    
            Username = username;
            Password = password;
        }
        public string? GetPassword() => _password;
    }
}
