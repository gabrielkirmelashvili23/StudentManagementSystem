using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // ადმინი - ლექტორის ყველა ფუნქცია + ლექტორების მართვა
    internal class Admin : User
    {
        public Admin() : base(UserRole.Admin)
        {

        }

        public Admin(int id, string name, string username, string password)
            : base(id, name, username, password, UserRole.Admin)
        {

        }

        // ShowInfo ადმინისთვის
        public override void ShowInfo()
        {
            Console.WriteLine($"[ADMINI] {Name} | username: {Username}");
        }
    }
}
