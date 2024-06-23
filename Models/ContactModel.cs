namespace CMS_.Introduction.Models
{
    public class ContactModel
    {
        public ContactModel(Guid id, string firstName, string lastName, string phoneNumber, string email, string notes)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Notes = notes;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }
    }
}
