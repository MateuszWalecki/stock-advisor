using System;
using FluentAssertions;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Exceptions;
using Xunit;

namespace StockAdvisor.UnitTests.Core
{
    public class UserTests
    {
        string validEmail = "valid@example.com",
            validFirstName = "John",
            validSurName = "Rambo",
            validPassword = "Example1";
        Guid guid = Guid.NewGuid();

        public User GetValidUser()
            => new User(guid, validEmail, validFirstName, validSurName, validPassword, "salt");
        
        [Fact]
        public void user_can_be_created_if_given_data_is_valid()
        {
            //Given

            //When
            var user = new User(guid, validEmail, validFirstName, validSurName, validPassword, "salt");

            //Then
            Assert.Equal(user.Id, guid);
            Assert.Equal(user.Email, validEmail);
            Assert.Equal(user.FirstName, validFirstName);
            Assert.Equal(user.SurName, validSurName);
            Assert.Equal(user.Password, validPassword);
        }

        [Fact]
        public void given_invalid_email_while_creating_user_throws_domain_exception()
        {
            //Given
            
            //When
            Action act = () =>
                new User(guid, "invalidEmail", validFirstName, validSurName, validPassword, "salt");

            //Then
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void given_invalid_firstname_while_creating_user_throws_domain_exception()
        {
            //Given
            
            //When
            Action act = () =>
                new User(guid, validEmail, "", validSurName, validPassword, "salt");

            //Then
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void given_invalid_surname_while_creating_user_throws_domain_exception()
        {
            //Given
            
            //When
            Action act = () =>
                new User(guid, validEmail, validFirstName, "", validPassword, "salt");

            //Then
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void given_invalid_password_while_creating_user_throws_domain_exception()
        {
            //Given
            
            //When
            Action act = () =>
                new User(guid, validEmail, validFirstName, validSurName, "", "salt");

            //Then
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void users_email_can_be_changed()
        {
            //Given
            var user = GetValidUser();
            string newEmail = "new@example.com";

            //When
            user.SetEmail(newEmail);

            //Then
            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void users_email_are_saved_as_lower_invariant()
        {
            //Given
            var user = GetValidUser();
            string newEmail = "New@example.com";

            //When
            user.SetEmail(newEmail);

            //Then
            Assert.Equal(newEmail.ToLowerInvariant(), user.Email);
        }

        [Fact]
        public void users_first_name_can_be_changed()
        {
            //Given
            var user = GetValidUser();
            string newFirstName = "Sylvester";

            //When
            user.SetFirstName(newFirstName);

            //Then
            Assert.Equal(newFirstName, user.FirstName);
        }

        [Fact]
        public void users_surname_can_be_changed()
        {
            //Given
            var user = GetValidUser();
            string newSurname = "Stalone";

            //When
            user.SetSurName(newSurname);

            //Then
            Assert.Equal(newSurname, user.SurName);
        }
        
        [Fact]
        public void users_password_can_be_changed()
        {
            //Given
            var user = GetValidUser();
            string newPw = "newPassword2";

            //When
            user.SetPassword(newPw, "newSalt");

            //Then
            Assert.Equal(newPw, user.Password);
        }
    }
}