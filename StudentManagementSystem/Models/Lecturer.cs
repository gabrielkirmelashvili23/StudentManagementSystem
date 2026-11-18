using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // ლექტორი - User-ს ემატება საგანი
    internal class Lecturer : User
    {
        public string Subject { get; set; } = string.Empty;

        public Lecturer() : base(UserRole.Lecturer)
        {

        }

        public Lecturer(int id, string name, string username, string password, string subject)
            : base(id, name, username, password, UserRole.Lecturer)
        {
            Subject = subject;
        }

        // ჯერ მშობლის ShowInfo, მერე საგანი
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"     sagani: {Subject}");
        }

        public override string ToFileLine()
        {
            return base.ToFileLine() + $"|{Subject}";
        }
    }
}
