using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // ადმინისტრატორის კლასი - მემკვიდრეობით იღებს User კლასს.
    // ადმინს შეუძლია ყველაფერი, რაც ლექტორს + ლექტორების მართვა
    internal class Admin : User
    {
        // ცარიელი კონსტრუქტორი - მშობელს გადასცემს მხოლოდ როლს
        public Admin() : base(UserRole.Admin)
        {

        }

        // სრული კონსტრუქტორი - მონაცემებს გადასცემს მშობელ User კლასს (base)
        public Admin(int id, string name, string username, string password)
            : base(id, name, username, password, UserRole.Admin)
        {

        }

        // ShowInfo მეთოდის გადაწერა (override) - ადმინისთვის საკუთარი ფორმატით
        public override void ShowInfo()
        {
            Console.WriteLine($"[ADMINI] {Name} | username: {Username}");
        }
    }
}
