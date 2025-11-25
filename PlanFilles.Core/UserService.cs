using System.Text;
namespace PlainFiles.Core;

public class UserService
{
    private readonly string _path;
    private List<User> _users = new();
    public UserService(string path)
    {
        _path = path;
        if (!File.Exists(_path))
        {
            using var fs = File.Create(_path);
        }
        LoadUsers();
    }
    private void LoadUsers()
    {
        _users = File.ReadAllLines(_path, Encoding.UTF8).Where(line => !string.IsNullOrWhiteSpace(line)).Select(line =>
            {
                var parts = line.Split(',');
                return new User
                {
                    Username = parts[0],
                    Password = parts[1],
                    IsActive = bool.Parse(parts[2])
                };
            }).ToList();
    }
        private void SaveUsers()
    {
        var lines = _users.Select(u => $"{u.Username},{u.Password},{u.IsActive}").ToArray();
        File.WriteAllLines(_path, lines, Encoding.UTF8);
    }

    public User? ValidateUser(string username, string password)
    {
        return _users.FirstOrDefault(u =>u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) 
        && u.Password == password && u.IsActive);
    }

    public bool UserExists(string username)
    {
        return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public void BlockUser(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user != null)
        {
            user.IsActive = false;
            SaveUsers();
        }
    }
}

