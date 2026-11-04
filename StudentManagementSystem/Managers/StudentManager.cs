using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Managers
{
    // კლასი, რომელიც მართავს სტუდენტებს: დამატება, ნახვა, ძებნა,
    // შეფასების განახლება, წაშლა და სტატისტიკა
    internal class StudentManager
    {
        // ყველა მომხმარებლის საერთო სია (ადმინები, ლექტორები, სტუდენტები).
        // ერთი და იგივე სია გადაეცემა ყველა მენეჯერს, რომ მონაცემები არ გაორდეს
        private List<User> _users;

        // ცარიელი კონსტრუქტორი - ქმნის ახალ ცარიელ სიას
        public StudentManager()
        {
            _users = new List<User>();
        }

        // სრული კონსტრუქტორი - იღებს უკვე არსებულ საერთო სიას
        public StudentManager(List<User> users)
        {
            _users = users;
        }

        // მეთოდი ამატებს ახალ სტუდენტს.
        // username და პაროლი ავტომატურად იქმნება სიის ნომრით (მაგ. s101 / student_101)
        public Student AddStudent(string name, int rollNumber, char grade)
        {
            // სიის ნომერი უნიკალური უნდა იყოს
            if (RollNumberExists(rollNumber))
                throw new DuplicateRollNumberException($"Roll number {rollNumber} is already taken!");

            string username = $"s{rollNumber}";
            string password = $"student_{rollNumber}";

            // ავტომატური username არ უნდა ემთხვეოდეს სხვა მომხმარებლის username-ს
            bool usernameTaken = _users.Any(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));

            if (usernameTaken)
                throw new DuplicateUsernameException($"Username {username} is already taken!");

            // ახალი Id = ყველაზე დიდი არსებული Id + 1
            int newId = 1;

            if (_users.Count > 0)
            {
                newId = _users.Max(user => user.Id) + 1;
            }

            Student newStudent = new Student(newId, name, username, password, rollNumber, grade);
            _users.Add(newStudent);

            return newStudent;
        }

        // მეთოდი ამოწმებს, არის თუ არა სიის ნომერი უკვე დაკავებული
        public bool RollNumberExists(int rollNumber)
        {
            return _users
                .OfType<Student>()
                .Any(student => student.RollNumber == rollNumber);
        }

        // მეთოდი აბრუნებს ყველა სტუდენტს, დალაგებულს სიის ნომრის მიხედვით.
        // OfType<Student>() საერთო სიიდან მხოლოდ სტუდენტებს ირჩევს
        public List<Student> GetAllStudents()
        {
            return _users
                .OfType<Student>()
                .OrderBy(student => student.RollNumber)
                .ToList();
        }

        // მეთოდი პოულობს სტუდენტს სიის ნომრით.
        // თუ ვერ იპოვა, ისვრის NotFoundException-ს
        public Student FindByRollNumber(int rollNumber)
        {
            foreach (Student student in GetAllStudents())
            {
                if (student.RollNumber == rollNumber)
                {
                    return student;
                }
            }

            throw new NotFoundException($"Student with roll number {rollNumber} not found!");
        }

        // მეთოდი ანახლებს სტუდენტის შეფასებას.
        // არასწორი შეფასების შემთხვევაში Student.UpdateGrade ისვრის InvalidGradeException-ს
        public Student UpdateGrade(int rollNumber, char newGrade)
        {
            Student student = FindByRollNumber(rollNumber);
            student.UpdateGrade(newGrade);

            return student;
        }

        // ===================== სტატისტიკა (LINQ და დელეგატები) =====================

        // მეთოდი აბრუნებს საშუალო ქულას (A = 5 ... F = 0). თუ სტუდენტი არ არის - 0
        public double GetAverageGradePoints()
        {
            List<Student> students = GetAllStudents();

            if (students.Count == 0)
            {
                return 0;
            }

            return students.Average(student => student.GetGradePoints());
        }

        // მეთოდი აჯგუფებს სტუდენტებს შეფასების მიხედვით (GroupBy), დალაგებულს A-დან F-მდე
        public List<IGrouping<char, Student>> GetStudentsGroupedByGrade()
        {
            return GetAllStudents()
                .GroupBy(student => student.Grade)
                .OrderBy(group => group.Key)
                .ToList();
        }

        // მეთოდი აბრუნებს საუკეთესო სტუდენტებს: ჯერ ქულით (კლებადობით), შემდეგ სახელით
        public List<Student> GetTopStudents(int count)
        {
            return GetAllStudents()
                .OrderByDescending(student => student.GetGradePoints())
                .ThenBy(student => student.Name)
                .Take(count)
                .ToList();
        }

        // მეთოდი აბრუნებს ჩაბარებულ სტუდენტებს (Predicate დელეგატი + FindAll)
        public List<Student> GetPassedStudents()
        {
            Predicate<Student> passed = student => student.HasPassed();

            return GetAllStudents().FindAll(passed);
        }

        // მეთოდი აბრუნებს ვერ ჩაბარებულ სტუდენტებს (ლამბდა პირდაპირ FindAll-ში)
        public List<Student> GetFailedStudents()
        {
            return GetAllStudents().FindAll(student => !student.HasPassed());
        }

        // მეთოდი შლის სტუდენტს სიიდან და აბრუნებს წაშლილ სტუდენტს
        public Student DeleteStudent(int rollNumber)
        {
            Student student = FindByRollNumber(rollNumber);
            _users.Remove(student);

            return student;
        }
    }
}
