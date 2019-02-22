namespace Workflow.Core.Data
{
    public class Address : IStepData
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }

        public Address(string street, string city, string state, string zip)
        {
            Street = street;
            City = city;
            State = state;
            Zip = zip;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Street) &&
                   !string.IsNullOrEmpty(City) &&
                   !string.IsNullOrEmpty(State) &&
                   !string.IsNullOrEmpty(Zip);
        }
    }
}