using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // ლექტორის კლასი - მემკვიდრეობით იღებს User კლასს და ამატებს საგანს
    internal class Lecturer : User
    {
        // საგანი, რომელსაც ლექტორი ასწავლის
        public string Subject { get; set; } = string.Empty;

        // ცარიელი კონსტრუქტორი - მშობელს გადასცემს მხოლოდ როლს
        public Lecturer() : base(UserRole.Lecturer)
        {

        }

        // სრული კონსტრუქტორი - საერთო მონაცემებს გადასცემს მშობელს (base),
        // საკუთარ ველს (Subject) კი თვითონ ავსებს
        public Lecturer(int id, string name, string username, string password, string subject)
            : base(id, name, username, password, UserRole.Lecturer)
        {
            Subject = subject;
        }

        // ShowInfo მეთოდის გადაწერა (override):
        // ჯერ ვიძახებთ მშობლის ვერსიას (base.ShowInfo), შემდეგ ვამატებთ საგანს
        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"     sagani: {Subject}");
        }

        // ToFileLine მეთოდის გადაწერა (override): მშობლის ხაზს ემატება საგანი
        public override string ToFileLine()
        {
            return base.ToFileLine() + $"|{Subject}";
        }
    }
}
