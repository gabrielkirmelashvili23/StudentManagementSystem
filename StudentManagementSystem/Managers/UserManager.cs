using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Managers
{
    // შესვლა (login) და ლექტორების მართვა
    internal class UserManager
    {
        private List<User> _users;

        public UserManager()
        {
            _users = new List<User>();
        }

        public UserManager(List<User> users)
        {
            _users = users;
        }

        // Login - username-ში დიდ/პატარა ასოს მნიშვნელობა არ აქვს, პაროლში აქვს
        public User Login(string username, string password)
        {
            foreach (User user in _users)
            {
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase)
                    && user.CheckPassword(password))
                {
                    return user;
                }
            }

            throw new WrongPasswordException("Wrong username or password!");
        }

        public bool UsernameExists(string username)
        {
            return _users.Any(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        // AddLecturer
        public Lecturer AddLecturer(string name, string username, string password, string subject)
        {
            if (UsernameExists(username))
                throw new DuplicateUsernameException($"Username {username} is already taken!");

            int newId = 1;

            if (_users.Count > 0)
            {
                newId = _users.Max(user => user.Id) + 1;
            }

            Lecturer lecturer = new Lecturer(newId, name, username, password, subject);
            _users.Add(lecturer);

            return lecturer;
        }

        public List<User> GetAllUsers()
        {
            return _users
                .OrderBy(user => user.Role)
                .ThenBy(user => user.Id)
                .ToList();
        }

        public List<Lecturer> GetAllLecturers()
        {
            return _users
                .OfType<Lecturer>()
                .ToList();
        }

        // GetAllLecturers() ასლს აბრუნებს, ამიტომ foreach-ში წაშლა უსაფრთხოა
        public Lecturer DeleteLecturer(string username)
        {
            foreach (Lecturer lecturer in GetAllLecturers())
            {
                if (string.Equals(lecturer.Username, username, StringComparison.OrdinalIgnoreCase))
                {
                    _users.Remove(lecturer);
                    return lecturer;
                }
            }

            throw new NotFoundException($"Lecturer with username {username} not found!");
        }
    }
}
