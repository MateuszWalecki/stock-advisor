using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using StockAdvisor.Core.Exceptions;

namespace StockAdvisor.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string FirstName { get; protected set; }
        public string SurName { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected User()
        {
        }

        public User(Guid userId, string emailAddress, string firstName, 
             string surName,string password, string salt)
        {
            Id = userId;
            SetEmail(emailAddress);
            SetFirstName(firstName);
            SetSurName(surName);
            SetPassword(password, salt);
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
                throw new DomainException(ErrorCodes.InvalidEmail,
                    "Email is invalid.");
            }
            
            if (Email == emailAddress)
            {
                throw new DomainException(ErrorCodes.InvalidEmail,
                    "New email cannot be equal to the currently used.");
            }

            Email = emailAddress;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetFirstName(string firstName) 
        {
            if (String.IsNullOrEmpty(firstName))
            {
                throw new DomainException(ErrorCodes.InvalidFirstName, 
                    "First name is invalid.");
            }

            if (FirstName == firstName)
            {
                throw new DomainException(ErrorCodes.InvalidFirstName, 
                    "New first name cannot be equal to the currently used.");
            }

            FirstName = firstName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetSurName(string surName) 
        {
            if (String.IsNullOrEmpty(surName))
            {
                throw new DomainException(ErrorCodes.InvalidSurname, 
                    "Surname is invalid.");
            }

            if (SurName == surName)
            {
                throw new DomainException(ErrorCodes.InvalidSurname, 
                    "New surname cannot be equal to the currently used.");
            }

            SurName = surName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, 
                    "Password can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, 
                    "Salt can not be empty.");
            }
            if (password.Length < 4) 
            {
                throw new DomainException(ErrorCodes.InvalidPassword, 
                    "Password must contain at least 4 characters.");
            }
            if (password.Length > 100) 
            {
                throw new DomainException(ErrorCodes.InvalidPassword, 
                    "Password can not contain more than 100 characters.");
            }
            if (Password == password)
            {
                throw new DomainException(ErrorCodes.InvalidPassword, 
                    "New password cannot be equal to the currently used.");
            }

            Password = password;
            Salt = salt;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}