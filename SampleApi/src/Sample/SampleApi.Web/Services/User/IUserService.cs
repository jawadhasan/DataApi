using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApi.Web.Services.User
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string userName, string password, out User user);
        Task<bool> AddUser(string userName, string password);
    }

    public class BasicUserService : IUserService
    {
        private IDictionary<string, (string passwordHash, User user)> _users = new Dictionary<string, (string passwordHash, User user)>();

        public BasicUserService(IDictionary<string, string> users)
        {
            foreach (var user in users)
            {
                //_users.Add(user.Key.ToLower(), (BCrypt.Net.BCrypt.HashPassword(user.Value), new User(user.Key)));
                _users.Add(user.Key.ToLower(), (user.Value, new User(user.Key)));
            }
        }
        public Task<bool> ValidateCredentials(string userName, string password, out User user)
        {
            user = null;
            var key = userName.ToLower();
            if (_users.ContainsKey(key))
            {
                var hash = _users[key].passwordHash;
                //if (BCrypt.Net.BCrypt.Verify(password, hash))
                if (password.Equals(hash))
                {
                    user = _users[key].user;
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }

        public Task<bool> AddUser(string userName, string password)
        {
            if (_users.ContainsKey(userName.ToLower()))
            {
                return Task.FromResult(false);
            }
            // _users.Add(userName.ToLower(), (BCrypt.Net.BCrypt.HashPassword(password), new User(userName)));
            _users.Add(userName.ToLower(), (password, new User(userName)));
            return Task.FromResult(true);
        }
    }


    public class User
    {
        public string UserName { get; }

        public User(string userName)
        {
            UserName = userName;
        }
    }
}
