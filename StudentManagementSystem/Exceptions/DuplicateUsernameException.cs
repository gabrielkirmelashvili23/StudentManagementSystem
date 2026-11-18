namespace StudentManagementSystem.Exceptions
{
    // როცა username უკვე დაკავებულია
    internal class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException(string message) : base(message)
        {

        }
    }
}
