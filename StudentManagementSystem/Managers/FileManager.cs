using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Managers
{
    // ფაილებთან მუშაობა: users.txt და სტუდენტების report ფაილები
    internal class FileManager
    {
        private string _dataFolder;
        private string _usersFilePath;
        private string _reportsFolder;

        public FileManager()
        {
            _dataFolder = "Data";
            _usersFilePath = _dataFolder + "/users.txt";
            _reportsFolder = _dataFolder + "/Reports";
        }

        public FileManager(string dataFolder)
        {
            _dataFolder = dataFolder;
            _usersFilePath = _dataFolder + "/users.txt";
            _reportsFolder = _dataFolder + "/Reports";
        }

        // LoadUsers - პირველ გაშვებაზე ფაილი იქმნება სატესტო მონაცემებით
        public List<User> LoadUsers()
        {
            if (!Directory.Exists(_dataFolder))
            {
                Directory.CreateDirectory(_dataFolder);
            }

            if (!File.Exists(_usersFilePath))
            {
                List<User> defaultUsers = CreateDefaultUsers();
                SaveUsers(defaultUsers);

                Console.WriteLine("pirveli gashveba: sheiqmna faili users.txt satesto monacemebit.");
                return defaultUsers;
            }

            string[] lines = File.ReadAllLines(_usersFilePath);
            List<User> users = new List<User>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    // დაზიანებულ ხაზს ვტოვებთ, პროგრამა არ ჩერდება
                    try
                    {
                        User user = ParseUser(lines[i]);
                        users.Add(user);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"faili: {i + 1}-e xazi dazianebulia da gamotovebulia ({ex.Message})");
                    }
                }
            }

            return users;
        }

        // SaveUsers - ფაილს თავიდან წერს
        public void SaveUsers(List<User> users)
        {
            List<string> lines = users
                .Select(user => user.ToFileLine())
                .ToList();

            File.WriteAllLines(_usersFilePath, lines);
        }

        // report ფაილები: Data/Reports/Student_101.txt
        private string GetReportPath(int rollNumber)
        {
            return _reportsFolder + $"/Student_{rollNumber}.txt";
        }

        // CreateReport - ქმნის ან თავიდან წერს report ფაილს
        public string CreateReport(Student student)
        {
            if (!Directory.Exists(_reportsFolder))
            {
                Directory.CreateDirectory(_reportsFolder);
            }

            string status = "chaabara";

            if (!student.HasPassed())
            {
                status = "ver chaabara";
            }

            string content = "--- STUDENTIS REPORT CARD ---\n" +
                $"saxeli da gvari: {student.Name}\n" +
                $"siis nomeri:     {student.RollNumber}\n" +
                $"shefaseba:       {student.Grade} ({student.GetGradePoints()} qula 5-dan)\n" +
                $"statusi:         {status}";

            string path = GetReportPath(student.RollNumber);
            File.WriteAllText(path, content);

            return path;
        }

        public bool ReportExists(int rollNumber)
        {
            return File.Exists(GetReportPath(rollNumber));
        }

        // ReadReport
        public string ReadReport(int rollNumber)
        {
            if (!ReportExists(rollNumber))
                throw new NotFoundException($"Report card for roll number {rollNumber} not found!");

            return File.ReadAllText(GetReportPath(rollNumber));
        }

        // DeleteReport
        public bool DeleteReport(int rollNumber)
        {
            if (!ReportExists(rollNumber))
            {
                return false;
            }

            File.Delete(GetReportPath(rollNumber));
            return true;
        }

        // ხაზის ფორმატი: Role|Id|Name|Username|Password|...
        private User ParseUser(string line)
        {
            string[] parts = line.Split('|');

            string role = parts[0];
            int id = int.Parse(parts[1]);
            string name = parts[2];
            string username = parts[3];
            string password = parts[4];

            switch (role)
            {
                case "Admin":
                    return new Admin(id, name, username, password);
                case "Lecturer":
                    return new Lecturer(id, name, username, password, parts[5]);
                case "Student":
                    int rollNumber = int.Parse(parts[5]);
                    char grade = char.Parse(parts[6]);
                    return new Student(id, name, username, password, rollNumber, grade);
                default:
                    throw new FormatException($"Unknown role: {role}");
            }
        }

        // სატესტო მონაცემები პირველი გაშვებისთვის
        private List<User> CreateDefaultUsers()
        {
            List<User> users = new List<User>()
            {
                new Admin(1, "Gabriel Kirmelashvili", "admin", "admin_123"),
                new Lecturer(2, "Giorgi Beridze", "lecturer", "lecturer_123", "C# programireba"),
                new Student(3, "Nino Kapanadze", "s101", "student_101", 101, 'A'),
                new Student(4, "Luka Maisuradze", "s102", "student_102", 102, 'B'),
                new Student(5, "Ana Gelashvili", "s103", "student_103", 103, 'C'),
                new Student(6, "Saba Japaridze", "s104", "student_104", 104, 'B'),
                new Student(7, "Mariam Chikovani", "s105", "student_105", 105, 'E'),
                new Student(8, "Irakli Nozadze", "s106", "student_106", 106, 'F')
            };

            return users;
        }
    }
}
