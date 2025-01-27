using System.Reflection;
using ValidationAttribute.Attributes;
using ValidationAttribute.Roles;

namespace ValidationAttribute.ValidationHandler
{
    //Reflection
    public class Handler
    {
        public void Handle(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userType = user.GetType();
            var members = userType.GetMembers();

            foreach (var member in members)
            {
                ValidateMember(member, user);
            }
        }

        private void ValidateMember(MemberInfo member, User user)
        {
            foreach (var customAttribute in member.GetCustomAttributes())
            {
                if (customAttribute is EmailValidation emailValidation)
                {
                    ValidateEmail(member, user, emailValidation);
                }
                else if (customAttribute is PasswordValidation passwordValidation)
                {
                    ValidatePassword(member, user, passwordValidation);
                }
                else if (customAttribute is ValidationLength lengthValidation)
                {
                    ValidateLength(member, user, lengthValidation);
                }
            }
        }

        private void ValidateEmail(MemberInfo member, User user, EmailValidation emailValidation)
        {
            var value = GetValueOfPropertyOrField(member, user) as string;

            if (value == null || !value.Contains('@') || !(value.EndsWith("gmail.com") || value.EndsWith("mail.ru")) || value.Length < 10)
            {
                throw new ArgumentException($"Email validation failed for member {member.Name}");
            }
        }

        private void ValidatePassword(MemberInfo member, User user, PasswordValidation passwordValidation)
        {
            var value = GetValueOfPropertyOrField(member, user) as string;

            if (value == null || value.Length < passwordValidation.StartLength || value.Length > passwordValidation.EndLength)
            {
                throw new ArgumentException($"Password validation failed for member {member.Name}");
            }
        }

        private void ValidateLength(MemberInfo member, User user, ValidationLength lengthValidation)
        {
            var value = GetValueOfPropertyOrField(member, user) as string;

            if (value == null || value.Length < lengthValidation.LowLength || value.Length > lengthValidation.HighLength)
            {
                throw new ArgumentException($"Length validation failed for member {member.Name}");
            }
        }

        private object GetValueOfPropertyOrField(MemberInfo member, User user)
        {
            return member switch
            {
                PropertyInfo property => property.GetValue(user),
                FieldInfo field => field.GetValue(user),
                _ => null
            };
        }

    }   
}
