namespace StudentManagementSystem.Exceptions
{
    // საკუთარი გამონაკლისი (exception), რომელიც ისვრის მაშინ, როცა
    // მომხმარებლის სახელი ან პაროლი არასწორია
    internal class WrongPasswordException : Exception
    {
        // კონსტრუქტორი იღებს შეტყობინებას და გადასცემს მშობელ Exception კლასს
        public WrongPasswordException(string message) : base(message)
        {

        }
    }
}
