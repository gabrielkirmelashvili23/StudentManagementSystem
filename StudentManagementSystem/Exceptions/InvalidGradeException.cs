namespace StudentManagementSystem.Exceptions
{
    // საკუთარი გამონაკლისი (exception), რომელიც ისვრის მაშინ, როცა
    // შეფასება არ არის დასაშვები (A, B, C, D, E, F)
    internal class InvalidGradeException : Exception
    {
        // კონსტრუქტორი იღებს შეტყობინებას და გადასცემს მშობელ Exception კლასს
        public InvalidGradeException(string message) : base(message)
        {

        }
    }
}
