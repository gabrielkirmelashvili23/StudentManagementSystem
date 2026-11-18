using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models.Enums;

namespace StudentManagementSystem.Models
{
    // სტუდენტი - User-ს ემატება სიის ნომერი და შეფასება
    internal class Student : User
    {
        // დასაშვები შეფასებები
        public static char[] ValidGrades { get; private set; } = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };

        public int RollNumber { get; private set; }

        // შეფასება იცვლება მხოლოდ UpdateGrade-ით
        public char Grade { get; private set; }

        public Student() : base(UserRole.Student)
        {

        }

        public Student(int id, string name, string username, string password, int rollNumber, char grade)
            : base(id, name, username, password, UserRole.Student)
        {
            RollNumber = rollNumber;
            UpdateGrade(grade);
        }

        // IsValidGrade
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

        // UpdateGrade
        public void UpdateGrade(char newGrade)
        {
            if (!IsValidGrade(newGrade))
                throw new InvalidGradeException($"Grade must be one of: {string.Join(", ", ValidGrades)}!");

            Grade = newGrade;
        }

        // F = ვერ ჩააბარა
        public bool HasPassed()
        {
            return Grade != 'F';
        }

        // A = 5 ... F = 0, სტატისტიკისთვის
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

        public override void ShowInfo()
        {
            string status = "chaabara";

            if (!HasPassed())
            {
                status = "ver chaabara";
            }

            Console.WriteLine($"[N{RollNumber}] {Name} | shefaseba: {Grade} | {status}");
        }

        public override string ToFileLine()
        {
            return base.ToFileLine() + $"|{RollNumber}|{Grade}";
        }
    }
}
