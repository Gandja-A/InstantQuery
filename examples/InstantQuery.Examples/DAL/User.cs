namespace InstantQuery.Examples.DAL
{
    public enum Gender
    {
        Male,
        Female
    }

    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public Guid CartId { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public List<Order> Orders { get; set; }
    }
}
