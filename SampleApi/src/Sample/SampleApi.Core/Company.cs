namespace SampleApi.Core
{
    public class Company
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
    }
}
