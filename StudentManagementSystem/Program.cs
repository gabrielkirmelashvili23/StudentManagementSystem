using StudentManagementSystem.Exceptions;
using StudentManagementSystem.Managers;
using StudentManagementSystem.Models;
using StudentManagementSystem.Models.Enums;

FileManager fileManager = new FileManager();

List<User> users = fileManager.LoadUsers();
Console.WriteLine($"chaitvirta {users.Count} momxmarebeli.");

StudentManager studentManager = new StudentManager(users);
UserManager userManager = new UserManager(users);

Console.WriteLine("\n STUDENTEBIS MARTVIS SISTEMA ");

string mainMenuText = "\nmtavari menu: \n" +
    "1. shesvla\n" +
    "2. gasvla\n";

bool wantToExit = false;

// მთავარი მენიუ
do
{
    int userChoice = ReadMenuChoice(mainMenuText, 2);

    if (userChoice == 1)
    {
        LoginMenu();
    }
    else
    {
        wantToExit = true;
        Console.WriteLine("naxvamdis!");
    }
} while (!wantToExit);


// შესვლა, მაქსიმუმ 3 მცდელობა
void LoginMenu()
{
    int maxAttempts = 3;

    for (int attempt = 1; attempt <= maxAttempts; attempt++)
    {
        Console.Write("sheiyvane username: ");
        string username = (Console.ReadLine() ?? string.Empty).Trim();
        Console.Write("sheiyvane paroli: ");
        string password = Console.ReadLine() ?? string.Empty;

        try
        {
            User user = userManager.Login(username, password);
            Console.WriteLine($"\nmogesalmebit, {user.Name}! (roli: {user.Role})");

            switch (user.Role)
            {
                case UserRole.Admin:
                    StaffMenu(true);
                    break;
                case UserRole.Lecturer:
                    StaffMenu(false);
                    break;
                case UserRole.Student:
                    StudentMenu((Student)user);
                    break;
            }

            return;
        }
        catch (WrongPasswordException ex)
        {
            Console.WriteLine($"shecdoma: {ex.Message} (darchenili cdebi: {maxAttempts - attempt})");
        }
    }

    Console.WriteLine("cdebis limiti amowurulia, brundebit mtavar menushi.");
}


// ლექტორის მენიუ, ადმინს ემატება ლექტორების მართვა (isAdmin)
void StaffMenu(bool isAdmin)
{
    string menuText = "\nsheiyvane ricxvi shesabamisi moqmedebistvis: \n" +
        "1. axali studentis damateba\n" +
        "2. yvela studentis naxva\n" +
        "3. studentis dzebna nomrit\n" +
        "4. shefasebis ganaxleba\n" +
        "5. studentis washla\n" +
        "6. statistika\n" +
        "7. studentis report failis sheqmna\n";

    int logoutChoice = 8;

    if (isAdmin)
    {
        menuText += "8. axali leqtoris damateba\n" +
            "9. yvela momxmareblis naxva\n" +
            "10. leqtoris washla\n";
        logoutChoice = 11;
    }

    menuText += $"{logoutChoice}. gamosvla\n";

    bool loggedIn = true;

    do
    {
        int userChoice = ReadMenuChoice(menuText, logoutChoice);

        if (userChoice == logoutChoice)
        {
            loggedIn = false;
            Console.WriteLine("tqven gamoxvedit sistemidan.");
        }
        else
        {
            // შეცდომამ პროგრამა არ უნდა გააჩეროს
            try
            {
                switch (userChoice)
                {
                    case 1:
                        AddStudentMenu();
                        break;
                    case 2:
                        ShowAllStudentsMenu();
                        break;
                    case 3:
                        SearchStudentMenu();
                        break;
                    case 4:
                        UpdateGradeMenu();
                        break;
                    case 5:
                        DeleteStudentMenu();
                        break;
                    case 6:
                        StatisticsMenu();
                        break;
                    case 7:
                        CreateReportMenu();
                        break;
                    case 8:
                        AddLecturerMenu();
                        break;
                    case 9:
                        ShowAllUsersMenu();
                        break;
                    case 10:
                        DeleteLecturerMenu();
                        break;
                }
            }
            catch (DuplicateRollNumberException ex)
            {
                Console.WriteLine($"shecdoma: {ex.Message}");
            }
            catch (DuplicateUsernameException ex)
            {
                Console.WriteLine($"shecdoma: {ex.Message}");
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"shecdoma: {ex.Message}");
            }
            catch (InvalidGradeException ex)
            {
                Console.WriteLine($"shecdoma: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"gautvaliswinebeli shecdoma: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("----------------------------------------");
            }
        }
    } while (loggedIn);
}

// AddStudentMenu
void AddStudentMenu()
{
    string name = ReadText("sheiyvane studentis saxeli da gvari: ", 2);
    int rollNumber = ReadPositiveNumber("sheiyvane siis nomeri: ");

    // ნომერს მაშინვე ვამოწმებთ, შეფასებამდე
    while (studentManager.RollNumberExists(rollNumber) || userManager.UsernameExists($"s{rollNumber}"))
    {
        Console.WriteLine($"siis nomeri {rollNumber} ukve dakavebulia, sheiyvanet sxva.");
        rollNumber = ReadPositiveNumber("sheiyvane siis nomeri: ");
    }

    char grade = ReadGrade("sheiyvane shefaseba (A, B, C, D, E, F): ");

    Student student = studentManager.AddStudent(name, rollNumber, grade);
    fileManager.SaveUsers(users);

    Console.WriteLine("studenti warmatebit daemata!");
    Console.WriteLine($"shesvlis monacemebi -> username: {student.Username}, paroli: {student.Password}");
}

// ShowAllStudentsMenu
void ShowAllStudentsMenu()
{
    List<Student> students = studentManager.GetAllStudents();

    if (students.Count == 0)
    {
        Console.WriteLine("studentebi jer ar aris damatebuli.");
        return;
    }

    Console.WriteLine($"--- yvela studenti ({students.Count}) ---");

    foreach (Student student in students)
    {
        student.ShowInfo();
    }
}

// SearchStudentMenu
void SearchStudentMenu()
{
    int rollNumber = ReadPositiveNumber("sheiyvane saziebeli studentis siis nomeri: ");
    Student student = studentManager.FindByRollNumber(rollNumber);

    Console.WriteLine("studenti moidzebna:");
    student.ShowInfo();
}

// UpdateGradeMenu
void UpdateGradeMenu()
{
    int rollNumber = ReadPositiveNumber("sheiyvane studentis siis nomeri: ");

    Student student = studentManager.FindByRollNumber(rollNumber);
    Console.WriteLine($"{student.Name}-is mimdinare shefaseba: {student.Grade}");

    char newGrade = ReadGrade("sheiyvane axali shefaseba (A, B, C, D, E, F): ");
    studentManager.UpdateGrade(rollNumber, newGrade);
    fileManager.SaveUsers(users);

    Console.WriteLine("shefaseba warmatebit ganaxlda!");
    student.ShowInfo();

    // report-ში ძველი შეფასება რომ არ დარჩეს
    if (fileManager.ReportExists(rollNumber))
    {
        fileManager.CreateReport(student);
        Console.WriteLine("report faili ganaxlda axali shefasebit.");
    }
}

// DeleteStudentMenu
void DeleteStudentMenu()
{
    int rollNumber = ReadPositiveNumber("sheiyvane wasashleli studentis siis nomeri: ");

    Student student = studentManager.FindByRollNumber(rollNumber);
    student.ShowInfo();

    if (!AskYesNo("namdvilad gsurt am studentis washla? (yes/no): "))
    {
        Console.WriteLine("washla gauqmda.");
        return;
    }

    studentManager.DeleteStudent(rollNumber);
    fileManager.SaveUsers(users);

    Console.WriteLine($"studenti {student.Name} warmatebit waishala!");

    // report ფაილიც იშლება
    if (fileManager.DeleteReport(rollNumber))
    {
        Console.WriteLine("studentis report faili waishala.");
    }
}

// StatisticsMenu
void StatisticsMenu()
{
    List<Student> students = studentManager.GetAllStudents();

    if (students.Count == 0)
    {
        Console.WriteLine("statistikistvis studentebi ar aris.");
        return;
    }

    Console.WriteLine("--- statistika ---");
    Console.WriteLine($"studentebis raodenoba: {students.Count}");

    double average = studentManager.GetAverageGradePoints();
    Console.WriteLine($"sashualo qula: {average:F2} (A = 5 ... F = 0)");

    // თითო სტუდენტზე ერთი *
    Console.WriteLine("\n--- shefasebebis ganawileba ---");

    foreach (IGrouping<char, Student> group in studentManager.GetStudentsGroupedByGrade())
    {
        Console.Write($"{group.Key}: ");

        foreach (Student student in group)
        {
            Console.Write("*");
        }

        Console.WriteLine($" ({group.Count()})");
    }

    Console.WriteLine("\n--- top 3 studenti ---");
    List<Student> topStudents = studentManager.GetTopStudents(3);

    for (int i = 0; i < topStudents.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {topStudents[i].Name} - {topStudents[i].Grade}");
    }

    List<Student> passedStudents = studentManager.GetPassedStudents();
    List<Student> failedStudents = studentManager.GetFailedStudents();

    Console.WriteLine($"\nchaabara: {passedStudents.Count}, ver chaabara: {failedStudents.Count}");

    foreach (Student student in failedStudents)
    {
        Console.WriteLine($"   ver chaabara: {student.Name} (N{student.RollNumber})");
    }
}

// CreateReportMenu
void CreateReportMenu()
{
    int rollNumber = ReadPositiveNumber("sheiyvane studentis siis nomeri: ");
    Student student = studentManager.FindByRollNumber(rollNumber);

    string path = fileManager.CreateReport(student);

    Console.WriteLine($"report faili warmatebit sheiqmna: {path}\n");
    Console.WriteLine(fileManager.ReadReport(rollNumber));
}

// AddLecturerMenu (ადმინი)
void AddLecturerMenu()
{
    string name = ReadText("sheiyvane leqtoris saxeli da gvari: ", 2);
    string username = ReadText("sheiyvane leqtoris username: ", 3);

    while (userManager.UsernameExists(username))
    {
        Console.WriteLine($"username {username} ukve dakavebulia, sheiyvanet sxva.");
        username = ReadText("sheiyvane leqtoris username: ", 3);
    }

    string password = ReadText("sheiyvane paroli (minimum 8 simbolo): ", 8);
    string subject = ReadText("sheiyvane sagani: ", 2);

    userManager.AddLecturer(name, username, password, subject);
    fileManager.SaveUsers(users);

    Console.WriteLine("leqtori warmatebit daemata!");
}

// ShowAllUsersMenu (ადმინი) - ShowInfo() ყველა ტიპზე თავისებურად მუშაობს
void ShowAllUsersMenu()
{
    List<User> allUsers = userManager.GetAllUsers();

    Console.WriteLine($"--- yvela momxmarebeli ({allUsers.Count}) ---");

    foreach (User user in allUsers)
    {
        user.ShowInfo();
    }
}

// DeleteLecturerMenu (ადმინი)
void DeleteLecturerMenu()
{
    List<Lecturer> lecturers = userManager.GetAllLecturers();

    if (lecturers.Count == 0)
    {
        Console.WriteLine("leqtorebi ar aris.");
        return;
    }

    Console.WriteLine("--- leqtorebi ---");

    foreach (Lecturer lecturer in lecturers)
    {
        lecturer.ShowInfo();
    }

    string username = ReadText("sheiyvane wasashleli leqtoris username: ", 1);

    if (!AskYesNo("namdvilad gsurt am leqtoris washla? (yes/no): "))
    {
        Console.WriteLine("washla gauqmda.");
        return;
    }

    Lecturer deletedLecturer = userManager.DeleteLecturer(username);
    fileManager.SaveUsers(users);

    Console.WriteLine($"leqtori {deletedLecturer.Name} warmatebit waishala!");
}


// სტუდენტის მენიუ - მხოლოდ საკუთარი მონაცემები
void StudentMenu(Student student)
{
    string menuText = "\nsheiyvane ricxvi shesabamisi moqmedebistvis: \n" +
        "1. chemi monacemebi da shefaseba\n" +
        "2. chemi report card\n" +
        "3. gamosvla\n";

    bool loggedIn = true;

    do
    {
        int userChoice = ReadMenuChoice(menuText, 3);

        switch (userChoice)
        {
            case 1:
                Console.WriteLine("--- chemi monacemebi ---");
                student.ShowInfo();
                Console.WriteLine($"username: {student.Username}");
                break;
            case 2:
                // report შეიძლება ჯერ არ იყოს შექმნილი
                try
                {
                    Console.WriteLine(fileManager.ReadReport(student.RollNumber));
                }
                catch (NotFoundException)
                {
                    Console.WriteLine("tqveni report card jer ar aris sheqmnili, mimartet leqtors.");
                }
                break;
            case 3:
                loggedIn = false;
                Console.WriteLine("tqven gamoxvedit sistemidan.");
                break;
        }
    } while (loggedIn);
}


// ვალიდაცია

// ReadMenuChoice - არასწორზე თავიდან ეკითხება
int ReadMenuChoice(string menuText, int maxChoice)
{
    Console.WriteLine(menuText);
    int.TryParse(Console.ReadLine(), out int userChoice);

    while (userChoice < 1 || userChoice > maxChoice)
    {
        Console.WriteLine("dafiqsirda araswori monacemi, scadet tavidan.");
        Console.WriteLine(menuText);
        int.TryParse(Console.ReadLine(), out userChoice);
    }

    return userChoice;
}

// ReadPositiveNumber
int ReadPositiveNumber(string message)
{
    Console.Write(message);
    int.TryParse(Console.ReadLine(), out int number);

    while (number <= 0)
    {
        Console.WriteLine("dafiqsirda araswori monacemi, unda sheiyvanot dadebiti ricxvi.");
        Console.Write(message);
        int.TryParse(Console.ReadLine(), out number);
    }

    return number;
}

string ReadText(string message, int minLength)
{
    Console.Write(message);
    string text = (Console.ReadLine() ?? string.Empty).Trim();

    while (text.Length < minLength || text.Contains("|"))
    {
        Console.WriteLine($"teqsti unda iyos minimum {minLength} simbolo da ar unda sheicavdes '|' simbolos.");
        Console.Write(message);
        text = (Console.ReadLine() ?? string.Empty).Trim();
    }

    return text;
}

// ReadGrade - a -> A
char ReadGrade(string message)
{
    Console.Write(message);
    string input = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

    while (input.Length != 1 || !Student.IsValidGrade(input[0]))
    {
        Console.WriteLine("araswori shefaseba! dasashvebia mxolod: A, B, C, D, E, F.");
        Console.Write(message);
        input = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();
    }

    return input[0];
}

// AskYesNo
bool AskYesNo(string message)
{
    Console.Write(message);
    string answer = (Console.ReadLine() ?? string.Empty).Trim();

    return string.Equals(answer, "yes", StringComparison.OrdinalIgnoreCase);
}
