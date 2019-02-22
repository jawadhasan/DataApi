namespace Workflow.Core.Data
{
    public class FormData : IStepData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get;set; }
        public string Work { get; set; }
        public string Street { get; set; }
        public string City { get;set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool IsValid { get; set; }

        public FormData()
        {
        }

        public FormData(string firstName, string lastName, string email, string work, string street, string city,
            string state, string zip, bool isValid = false)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Work = work;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
            IsValid = isValid;
        }
    }
}