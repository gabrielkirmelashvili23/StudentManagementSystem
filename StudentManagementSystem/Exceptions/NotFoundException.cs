namespace StudentManagementSystem.Exceptions
{
    // საკუთარი გამონაკლისი (exception), რომელიც ისვრის მაშინ, როცა
    // ვერ მოიძებნა (მაგ. სტუდენტი ასეთი ნომრით არ არსებობს)
    internal class NotFoundException : Exception
    {
        // კონსტრუქტორი იღებს შეტყობინებას და გადასცემს მშობელ Exception კლასს
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
