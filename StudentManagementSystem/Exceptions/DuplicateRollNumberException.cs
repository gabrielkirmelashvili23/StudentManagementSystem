namespace StudentManagementSystem.Exceptions
{
    // საკუთარი გამონაკლისი (exception), რომელიც ისვრის მაშინ, როცა
    // სიის ნომერი უკვე დაკავებულია სხვა სტუდენტის მიერ
    internal class DuplicateRollNumberException : Exception
    {
        // კონსტრუქტორი იღებს შეტყობინებას და გადასცემს მშობელ Exception კლასს
        public DuplicateRollNumberException(string message) : base(message)
        {

        }
    }
}
