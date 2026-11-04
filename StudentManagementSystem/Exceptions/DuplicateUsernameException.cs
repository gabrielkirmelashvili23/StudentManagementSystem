namespace StudentManagementSystem.Exceptions
{
    // საკუთარი გამონაკლისი (exception), რომელიც ისვრის მაშინ, როცა
    // მომხმარებლის სახელი (username) უკვე დაკავებულია
    internal class DuplicateUsernameException : Exception
    {
        // კონსტრუქტორი იღებს შეტყობინებას და გადასცემს მშობელ Exception კლასს
        public DuplicateUsernameException(string message) : base(message)
        {

        }
    }
}
