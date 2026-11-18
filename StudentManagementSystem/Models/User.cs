using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // მშობელი კლასი - Admin, Lecturer და Student მისგან იღებენ მემკვიდრეობით
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        // პაროლი უნდა იყოს მინიმუმ 8 სიმბოლო
        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 8)
                {
                    Console.WriteLine("paroli unda iyos minimum 8 simbolo!");
                }
                else
                {
                    _password = value;
                }
            }
        }

        // როლს მხოლოდ კონსტრუქტორი ანიჭებს
        public UserRole Role { get; private set; }

        public User()
        {

        }

        // ამას იძახებენ შვილობილი კლასების ცარიელი კონსტრუქტორები
        public User(UserRole role)
        {
            Role = role;
        }

        public User(int id, string name, string username, string password, UserRole role)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }

        // CheckPassword - პაროლი მოწმდება კლასის შიგნით
        public bool CheckPassword(string password)
        {
            return _password == password;
        }

        // ShowInfo - შვილობილი კლასები ამას გადაწერენ (override)
        public virtual void ShowInfo()
        {
            Console.WriteLine($"[{Id}] {Name} | username: {Username} | roli: {Role}");
        }

        // ToFileLine - ფაილში შესანახი ხაზი, ველები '|'-ით
        public virtual string ToFileLine()
        {
            return $"{Role}|{Id}|{Name}|{Username}|{Password}";
        }

        // ToString
        public override string ToString()
        {
            return $"[{Id}]: {Name}, {Role}.";
        }
    }
}
