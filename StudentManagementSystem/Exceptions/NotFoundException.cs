namespace StudentManagementSystem.Exceptions
{
    // როცა სტუდენტი, ლექტორი ან ფაილი ვერ მოიძებნა
    internal class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
