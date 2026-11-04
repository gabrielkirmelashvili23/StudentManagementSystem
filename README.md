# Student Management System (C# Console App)

**სტუდენტების მართვის სისტემა** — Step Academy, BackEnd (C#) პირველი ეტაპის შემაჯამებელი პროექტი.

Author: **Gabriel Kirmelashvili**

A console application for managing students, grades and report cards, with three user roles
(Admin, Lecturer, Student). All data is saved in text files, so nothing is lost when the program closes.

---

## Features / ფუნქციონალი

### Lecturer menu (ლექტორი)
| # | Option | Description |
|---|--------|-------------|
| 1 | axali studentis damateba | Add a student (name, roll number, grade). Login is created automatically: `s<roll>` / `student_<roll>` |
| 2 | yvela studentis naxva | View all students |
| 3 | studentis dzebna nomrit | Search a student by roll number |
| 4 | shefasebis ganaxleba | Update a student's grade |
| 5 | studentis washla | Delete a student (with confirmation) + delete the report file |
| 6 | statistika | Statistics: count, average, grade chart, top 3, passed / failed |
| 7 | studentis report failis sheqmna | Create a report-card file `Data/Reports/Student_<roll>.txt` |
| 8 | gamosvla | Log out |

Options 1–4 and logout/exit are the official requirements of the task; 5–7 are additions.

### Admin menu (ადმინი)
Everything the lecturer can do, plus:
- **8.** add a lecturer  **9.** view all users  **10.** delete a lecturer  **11.** log out

### Student menu (სტუდენტი)
- **1.** view my data and grade  **2.** view my report card  **3.** log out

---

## How to run / როგორ გავუშვათ

1. Open `StudentManagementSystem.slnx` in **Visual Studio** (requires **.NET 10**).
2. Press **F5**.
3. On the first run, the program creates `Data/users.txt` with sample data.

### Default logins
| Role | Username | Password |
|------|----------|----------|
| Admin | `admin` | `admin_123` |
| Lecturer | `lecturer` | `lecturer_123` |
| Student | `s101` … `s106` | `student_101` … `student_106` |

> To reset to the sample data, delete the `Data` folder (in Visual Studio it is in `StudentManagementSystem/bin/Debug/net10.0/Data`).

---

## Task criteria / შეფასების კრიტერიუმები

| Criterion | Where it is used |
|-----------|------------------|
| **2.1.1** Different variable types | `int RollNumber`, `string Name`, `char Grade`, `double` average, `bool` flags, `enum UserRole` |
| **2.1.2** Conditions and loops | `if/else`, `switch` and switch expressions (`Student.GetGradePoints`), `do-while` menus, `while` validation loops, `for` (login attempts, file lines), `foreach`, nested loop (grade chart) |
| **2.1.3** Arrays | `char[] ValidGrades` (`Student.cs`), `string[]` from `File.ReadAllLines` and `Split('\|')` (`FileManager.cs`) |
| **2.1.4** Own functions / methods | All methods in `StudentManager`, `UserManager`, `FileManager` and the menu/validation methods in `Program.cs` |
| **2.1.5** Create / edit / delete files | **Create:** `Directory.CreateDirectory`, `users.txt` on first run, report cards (`File.WriteAllText`). **Edit:** `users.txt` rewritten after every change (`File.WriteAllLines`), report rewritten when a grade changes. **Delete:** `File.Delete` removes the report when a student is deleted |
| **2.1.6** Inheritance | `Admin`, `Lecturer`, `Student` inherit from `User` and call `base(...)` |
| **2.1.6** Encapsulation | `private string _password` + validating setter, `CheckPassword()`, `private set` on `Role`, `RollNumber`, `Grade` (grade changes only through `UpdateGrade()`) |
| **2.1.6** Polymorphism | `virtual ShowInfo()` and `virtual ToFileLine()` in `User`, overridden in every child class; `override ToString()`. One `List<User>` calls `user.ShowInfo()` and each type prints differently |
| **2.1.7** Comments | Every class, method and important block is commented in Georgian |

Also used: custom exceptions + `try/catch/finally`, `List<T>`, LINQ (`OfType`, `OrderBy`, `ThenBy`, `GroupBy`, `Take`, `Average`, `Any`, `Max`, `Select`), `Predicate<T>` delegates with lambdas.

---

## Project structure / პროექტის სტრუქტურა

```
StudentManagementSystem/
├── Program.cs                 main menu, login, role menus, input validation
├── Models/
│   ├── User.cs                base class (Id, Name, Username, Password, Role)
│   ├── Admin.cs               Admin : User
│   ├── Lecturer.cs            Lecturer : User (+ Subject)
│   ├── Student.cs             Student : User (+ RollNumber, Grade)
│   └── Enums/UserRole.cs      Admin, Lecturer, Student
├── Managers/
│   ├── StudentManager.cs      add / view / search / update / delete students, statistics
│   ├── UserManager.cs         login, add / view / delete lecturers
│   └── FileManager.cs         all file work (users.txt and report cards)
└── Exceptions/
    ├── NotFoundException.cs
    ├── InvalidGradeException.cs
    ├── DuplicateRollNumberException.cs
    ├── DuplicateUsernameException.cs
    └── WrongPasswordException.cs
```

### Data file format (`Data/users.txt`)
One user per line, fields separated by `|`:
```
Admin|1|Gabriel Kirmelashvili|admin|admin_123
Lecturer|2|Giorgi Beridze|lecturer|lecturer_123|C# programireba
Student|3|Nino Kapanadze|s101|student_101|101|A
```
Damaged lines are skipped with a message, so a broken file never crashes the program.

---

## Validation / ვალიდაცია
- Menu choices and numbers: re-asked until valid (`int.TryParse`)
- Roll number: positive and unique
- Grade: only A–F, lowercase is accepted and converted (`a` → `A`)
- Name: at least 2 characters, no `|`
- Password: at least 8 characters
- Login: maximum 3 attempts
- Delete: asks for confirmation (yes/no)

## Grading scale
A = 5, B = 4, C = 3, D = 2, E = 1, F = 0. Grades A–E pass, F fails.
