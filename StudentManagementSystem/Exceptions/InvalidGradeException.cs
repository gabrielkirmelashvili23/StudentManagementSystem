namespace StudentManagementSystem.Exceptions
{
    // როცა შეფასება არ არის A-F
    internal class InvalidGradeException : Exception
    {
        public InvalidGradeException(string message) : base(message)
        {

        }
    }
}
