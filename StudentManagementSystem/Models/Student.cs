using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // სტუდენტის კლასი - მემკვიდრეობით იღებს User კლასს
    // და ამატებს სიის ნომერს (RollNumber) და შეფასებას (Grade)
    internal class Student : User
    {
        // დასაშვები შეფასებების მასივი (სტატიკური - ერთია ყველა სტუდენტისთვის)
        public static char[] ValidGrades { get; private set; } = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };

        // სიის ნომერი - გარედან შეცვლა შეუძლებელია (private set)
        public int RollNumber { get; private set; }

        // შეფასება - იცვლება მხოლოდ UpdateGrade მეთოდით (ენკაფსულაცია)
        public char Grade { get; private set; }

        // ცარიელი კონსტრუქტორი - მშობელს გადასცემს მხოლოდ როლს
        public Student() : base(UserRole.Student)
        {

        }

        // სრული კონსტრუქტორი - საერთო მონაცემებს გადასცემს მშობელს (base),
        // სიის ნომერს და შეფასებას კი თვითონ ავსებს
        public Student(int id, string name, string username, string password, int rollNumber, char grade)
            : base(id, name, username, password, UserRole.Student)
        {
            RollNumber = rollNumber;
            UpdateGrade(grade);
        }

        // სტატიკური მეთოდი ამოწმებს, არის თუ არა შეფასება დასაშვები
        // (მასივზე იტერაცია foreach ციკლით)
        public static bool IsValidGrade(char grade)
        {
            foreach (char validGrade in ValidGrades)
            {
                if (validGrade == grade)
                {
                    return true;
                }
            }

            return false;
        }

        // მეთოდი ცვლის სტუდენტის შეფასებას.
        // თუ შეფასება არასწორია, ისვრის InvalidGradeException-ს
        public void UpdateGrade(char newGrade)
        {
            if (!IsValidGrade(newGrade))
                throw new InvalidGradeException($"Grade must be one of: {string.Join(", ", ValidGrades)}!");

            Grade = newGrade;
        }

        // მეთოდი ამოწმებს, ჩააბარა თუ არა სტუდენტმა (F = ვერ ჩააბარა)
        public bool HasPassed()
        {
            return Grade != 'F';
        }

        // მეთოდი შეფასებას გადაიყვანს ქულაში (A = 5 ... F = 0) სტატისტიკისთვის
        public int GetGradePoints()
        {
            int points = Grade switch
            {
                'A' => 5,
                'B' => 4,
                'C' => 3,
                'D' => 2,
                'E' => 1,
                _ => 0
            };

            return points;
        }

        // ShowInfo მეთოდის გადაწერა (override) - სტუდენტისთვის საკუთარი ფორმატით
        public override void ShowInfo()
        {
            string status = "chaabara";

            if (!HasPassed())
            {
                status = "ver chaabara";
            }

            Console.WriteLine($"[N{RollNumber}] {Name} | shefaseba: {Grade} | {status}");
        }

        // ToFileLine მეთოდის გადაწერა (override): მშობლის ხაზს ემატება სიის ნომერი და შეფასება
        public override string ToFileLine()
        {
            return base.ToFileLine() + $"|{RollNumber}|{Grade}";
        }
    }
}
