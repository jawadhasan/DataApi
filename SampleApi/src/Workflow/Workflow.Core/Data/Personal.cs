namespace Workflow.Core.Data
{
    public class Personal : IStepData
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        private Personal()
        {
        }
        public Personal(string firstName, string lastName, string email)
        {
            //think if we should put guards here...seems we should
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Email);
        }
    }
}