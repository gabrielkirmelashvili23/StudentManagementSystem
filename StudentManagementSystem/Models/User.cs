using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // საბაზო (მშობელი) კლასი ყველა მომხმარებლისთვის.
    // მისგან მემკვიდრეობით იღებენ: Admin, Lecturer და Student
    internal class User
    {
        // მომხმარებლის უნიკალური იდენტიფიკატორი
        public int Id { get; set; }

        // მომხმარებლის სახელი და გვარი
        public string Name { get; set; } = string.Empty;

        // მომხმარებლის სახელი სისტემაში შესასვლელად (login)
        public string Username { get; set; } = string.Empty;

        // ენკაფსულაცია: პაროლი ინახება დახურულ (private) ველში,
        // გარედან მასთან წვდომა მხოლოდ Password property-ით არის შესაძლებელი
        private string _password = string.Empty;

        // Password property: set ამოწმებს, რომ პაროლი იყოს მინიმუმ 8 სიმბოლო
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

        // მომხმარებლის როლი - გარედან შეცვლა შეუძლებელია (private set),
        // მნიშვნელობას იღებს მხოლოდ კონსტრუქტორიდან
        public UserRole Role { get; private set; }

        // ცარიელი კონსტრუქტორი
        public User()
        {

        }

        // კონსტრუქტორი მხოლოდ როლით - იყენებენ შვილობილი კლასების ცარიელი კონსტრუქტორები
        public User(UserRole role)
        {
            Role = role;
        }

        // სრული კონსტრუქტორი
        public User(int id, string name, string username, string password, UserRole role)
        {
            Id = id;
            Name = name;
            Username = username;
            Password = password;
            Role = role;
        }

        // მეთოდი ამოწმებს, ემთხვევა თუ არა შეყვანილი პაროლი შენახულს.
        // ასე პაროლის შემოწმება ხდება კლასის შიგნით (ენკაფსულაცია)
        public bool CheckPassword(string password)
        {
            return _password == password;
        }

        // ვირტუალური მეთოდი (პოლიმორფიზმი):
        // virtual ნიშნავს, რომ შვილობილ კლასს შეუძლია ამ მეთოდის გადაწერა (override)
        public virtual void ShowInfo()
        {
            Console.WriteLine($"[{Id}] {Name} | username: {Username} | roli: {Role}");
        }

        // ვირტუალური მეთოდი: აბრუნებს მომხმარებლის მონაცემებს ფაილში შესანახად,
        // ველები გაყოფილია '|' სიმბოლოთი. შვილობილი კლასები ამატებენ საკუთარ ველებს
        public virtual string ToFileLine()
        {
            return $"{Role}|{Id}|{Name}|{Username}|{Password}";
        }

        // ToString მეთოდის გადაწერა - აბრუნებს მომხმარებლის მოკლე აღწერას
        public override string ToString()
        {
            return $"[{Id}]: {Name}, {Role}.";
        }
    }
}
