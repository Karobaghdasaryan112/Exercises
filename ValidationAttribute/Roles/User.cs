

using ValidationAttribute.Attributes;

namespace ValidationAttribute.Roles
{
    public class User
    {
        [ValidationLength(5, 10)]
        public string FirstName { get; set; }
        [ValidationLength(5, 10)]
        public string LastName { get; set; }

        [EmailValidation]
        public string Email { get; set; }
        [PasswordValidation(1,10)]
        public string Password { get; set; }
        public User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}
