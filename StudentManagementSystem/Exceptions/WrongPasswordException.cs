namespace StudentManagementSystem.Exceptions
{
    // როცა username ან პაროლი არასწორია
    internal class WrongPasswordException : Exception
    {
        public WrongPasswordException(string message) : base(message)
        {

        }
    }
}
