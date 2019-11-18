using System;
using System.Net.Mail;
using StockAdvisor.Core.Exceptions;

namespace StockAdvisor.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string FirstName { get; protected set; }
        public string SurName { get; protected set; }
        public UserRole Role { get; protected set; }
        public string PasswordHash { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected User()
        {
        }

        public User(Guid userId, string emailAddress, string firstName, 
             string surName, string passwordHash, string salt, UserRole userRole)
        {
            Id = userId;
            SetEmail(emailAddress);
            SetFirstName(firstName);
            SetSurName(surName);
            SetPassword(passwordHash, salt);
            SetRole(userRole);
            UpdatedAt = CreatedAt = DateTime.UtcNow;
        }


        public void SetEmail(string emailAddress)
        {
            emailAddress = emailAddress.ToLowerInvariant();

            try
            {
                var addr = new MailAddress(emailAddress);
            }
            catch
            {
                throw new InvalidEmailDomExc($"Email '{emailAddress}' is invalid.");
            }
            
            if (Email == emailAddress)
            {
                throw new InvalidEmailDomExc($"New email cannot be equal to the " +
                    "currently used.");
            }

            Email = emailAddress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetFirstName(string firstName) 
        {
            if (String.IsNullOrEmpty(firstName))
            {
                throw new InvalidFirstNameDomExc("First name cannot be null or empty.");
            }

            if (FirstName == firstName)
            {
                throw new InvalidFirstNameDomExc("New first name cannot be equal to " +
                    "the currently used.");
            }

            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSurName(string surName) 
        {
            if (String.IsNullOrEmpty(surName))
            {
                throw new InvalidSurNameDomExc("Surname cannot be null or empty.");
            }

            if (SurName == surName)
            {
                throw new InvalidSurNameDomExc("New surname cannot be equal to the currently" +
                    "used.");
            }

            SurName = surName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRole(UserRole role)
        {
            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string passwordHash, string salt)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash can not be null or empty.",
                    nameof(passwordHash));
            }
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentException("Salt can not be null or empty.", nameof(salt));
            }
            if (PasswordHash == passwordHash)
            {
                throw new ArgumentException("New and old password hashes cannot be equal.",
                    nameof(passwordHash));
            }
            if (Salt == salt)
            {
                throw new ArgumentException("New and old salts cannot be equal.",
                    nameof(salt));
            }

            PasswordHash = passwordHash;
            Salt = salt;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}