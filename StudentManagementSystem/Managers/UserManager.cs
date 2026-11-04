using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Managers
{
    // კლასი, რომელიც მართავს მომხმარებლებს: სისტემაში შესვლა (login)
    // და ლექტორების დამატება / ნახვა / წაშლა
    internal class UserManager
    {
        // ყველა მომხმარებლის საერთო სია (იგივე სია, რაც StudentManager-ს აქვს)
        private List<User> _users;

        // ცარიელი კონსტრუქტორი - ქმნის ახალ ცარიელ სიას
        public UserManager()
        {
            _users = new List<User>();
        }

        // სრული კონსტრუქტორი - იღებს უკვე არსებულ საერთო სიას
        public UserManager(List<User> users)
        {
            _users = users;
        }

        // მეთოდი ამოწმებს username-ს და პაროლს.
        // წარმატების შემთხვევაში აბრუნებს მომხმარებელს, თუ არა - ისვრის WrongPasswordException-ს
        public User Login(string username, string password)
        {
            foreach (User user in _users)
            {
                // username-ის შედარება დიდ/პატარა ასოებს არ ითვალისწინებს, პაროლის - ითვალისწინებს
                if (string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase)
                    && user.CheckPassword(password))
                {
                    return user;
                }
            }

            throw new WrongPasswordException("Wrong username or password!");
        }

        // მეთოდი ამოწმებს, არის თუ არა username უკვე დაკავებული
        public bool UsernameExists(string username)
        {
            return _users.Any(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        // მეთოდი ამატებს ახალ ლექტორს
        public Lecturer AddLecturer(string name, string username, string password, string subject)
        {
            if (UsernameExists(username))
                throw new DuplicateUsernameException($"Username {username} is already taken!");

            // ახალი Id = ყველაზე დიდი არსებული Id + 1
            int newId = 1;

            if (_users.Count > 0)
            {
                newId = _users.Max(user => user.Id) + 1;
            }

            Lecturer lecturer = new Lecturer(newId, name, username, password, subject);
            _users.Add(lecturer);

            return lecturer;
        }

        // მეთოდი აბრუნებს ყველა მომხმარებელს, დალაგებულს ჯერ როლით, შემდეგ Id-ით
        public List<User> GetAllUsers()
        {
            return _users
                .OrderBy(user => user.Role)
                .ThenBy(user => user.Id)
                .ToList();
        }

        // მეთოდი აბრუნებს მხოლოდ ლექტორებს
        public List<Lecturer> GetAllLecturers()
        {
            return _users
                .OfType<Lecturer>()
                .ToList();
        }

        // მეთოდი შლის ლექტორს username-ით. თუ ვერ იპოვა, ისვრის NotFoundException-ს
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
