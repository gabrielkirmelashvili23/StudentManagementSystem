namespace StudentManagementSystem.Exceptions
{
    // როცა სიის ნომერი უკვე დაკავებულია
    internal class DuplicateRollNumberException : Exception
    {
        public DuplicateRollNumberException(string message) : base(message)
        {

        }
    }
}
