using System;
using FluentAssertions;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Exceptions;
using Xunit;

namespace StockAdvisor.Tests.Unit.Core
{
    public class UserTests
    {
        private readonly string _validEmail = "valid@example.com",
            _validFirstName = "John",
            _validSurName = "Rambo",
            _validPasswordHash = "passwordfgsadgfg54664d1fg65daf65gd6sfg46",
            _validPasswordSalt = "saltg4fd8g4ds9f84g";
        private readonly UserRole _role = UserRole.User;

        private readonly Guid _guid = Guid.NewGuid();

        [Fact]
        public void user_can_be_created_if_given_data_is_valid()
        {
        //Given

        //When
            var user = new User(_guid, _validEmail, _validFirstName, _validSurName,
                _validPasswordHash, _validPasswordSalt, _role);

        //Then
            Assert.Equal(_guid, user.Id);
            Assert.Equal(_validEmail, user.Email);
            Assert.Equal(_validFirstName, user.FirstName);
            Assert.Equal(_validSurName, user.SurName);
            Assert.Equal(_validPasswordHash, user.PasswordHash);
            Assert.Equal(_role, user.Role);
        }

        [Fact]
        public void given_invalid_email_while_creating_user_throws_domain_exception()
        {
        //Given
            
        //When
            Action act = () =>
                new User(_guid, "invalidEmail", _validFirstName, _validSurName,
                    _validPasswordHash, _validPasswordSalt, _role);

        //Then
            Assert.Throws<InvalidEmailDomExc>(act);
        }

        [Fact]
        public void given_invalid_firstname_while_creating_user_throws_domain_exception()
        {
        //Given
            
        //When
            Action act = () =>
                new User(_guid, _validEmail, "", _validSurName, _validPasswordHash,
                    _validPasswordSalt, _role);

        //Then
            Assert.Throws<InvalidFirstNameDomExc>(act);
        }

        [Fact]
        public void given_invalid_surname_while_creating_user_throws_domain_exception()
        {
        //Given
            
        //When
            Action act = () =>
                new User(_guid, _validEmail, _validFirstName, "", _validPasswordHash,
                    _validPasswordSalt, _role);

        //Then
            Assert.Throws<InvalidSurNameDomExc>(act);
        }

        [Fact]
        public void given_invalid_password_hash_while_creating_user_throws_domain_exception()
        {
        //Given
            
        //When
            Action act = () =>
                new User(_guid, _validEmail, _validFirstName, _validSurName, "",
                    _validPasswordSalt, _role);

        //Then
            Assert.Throws<ArgumentException>(act);
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
        
        [Theory]
        [InlineData(UserRole.User)]
        [InlineData(UserRole.Admin)]
        public void user_role_can_be_set_while_creating_user(UserRole role)
        {
        //Given
            var user = new User(_guid, _validEmail, _validFirstName, _validSurName,
                _validPasswordHash, _validPasswordSalt, role);

        //When
            
        //Then
            user.Role.Should().BeEquivalentTo(role);
        }
        
        [Fact]
        public void user_role_can_be_changed()
        {
        //Given
            var user = GetValidUser();
            var defaultRole = user.Role;

        //When
            user.SetRole(UserRole.Admin);
            var newRole = user.Role;
            
        //Then
            user.Role.Should().BeEquivalentTo(UserRole.Admin);
            newRole.Should().NotBe(defaultRole);
        }

        [Fact]
        public void users_password_can_be_changed()
        {
        //Given
            var user = GetValidUser();
            string newPwHash = "newPasswordHash646464465";
            string newSalt = "newSalt4465d4saf654sd6f";

        //When
            user.SetPassword(newPwHash, newSalt);

        //Then
            Assert.Equal(newPwHash, user.PasswordHash);
            Assert.Equal(newSalt, user.Salt);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void given_invalid_hash_while_changing_password_func_throws_exception(string newPwHash)
        {
        //Given
            var user = GetValidUser();
            string newSalt = "newSalt4465d4saf654sd6f";

        //When
            Action act = () => user.SetPassword(newPwHash, newSalt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void given_invalid_salt_while_changing_password_func_throws_exception(string newSalt)
        {
        //Given
            var user = GetValidUser();
            string newPwHash = "newSalt4465d4saf654sd6f";

        //When
            Action act = () => user.SetPassword(newPwHash, newSalt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void when_given_hash_is_equal_to_current_while_changing_pw_func_throws_exception()
        {
        //Given
            var user = GetValidUser();
            string newPwHash = "newSalt4465d4saf654sd6f";

        //When
            Action act = () => user.SetPassword(newPwHash, _validPasswordSalt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void when_given_salt_is_equal_to_current_while_changing_pw_func_throws_exception()
        {
        //Given
            var user = GetValidUser();
            string newSalt = "newSalt4465d4saf654sd6f";

        //When
            Action act = () => user.SetPassword(_validPasswordHash, newSalt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void given_empy_null_hash_while_changing_it_throws_exception(string pwHash)
        {
        //Given
            var user = GetValidUser();
            string newSalt = "newSalt4465d4saf654sd6f";

        //When
            Action act = () => user.SetPassword(pwHash, newSalt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        private User GetValidUser()
            => new User(_guid, _validEmail, _validFirstName, _validSurName,
                _validPasswordHash, _validPasswordSalt, _role);
    }
}