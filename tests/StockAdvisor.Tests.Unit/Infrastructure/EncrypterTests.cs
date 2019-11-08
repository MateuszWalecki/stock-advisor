using System;
using FluentAssertions;
using StockAdvisor.Infrastructure.Services;
using Xunit;

namespace StockAdvisor.Tests.Unit.Infrastructure
{
    public class EncrypterTests
    {
        [Fact]
        public void get_salt_method_returns_random_based_string()
        {
        //Given
            var encrypter = new Encrypter();

        //When
            var salt1 = encrypter.GetSalt();
            var salt2 = encrypter.GetSalt();
            
        //Then
            salt1.Should().NotBeNullOrWhiteSpace();
            salt2.Should().NotBeNullOrWhiteSpace();
            salt1.Should().NotBe(salt2);
        }

        [Fact]
        public void get_hash_method_throws_argumentexception_if_given_password_is_empty()
        {
        //Given
            var encrypter = new Encrypter();
            string salt = encrypter.GetSalt();

        //When
            Action act = () => encrypter.GetHash("", salt);

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        
        [Fact]
        public void get_hash_method_throws_argumentexception_if_given_salt_is_empty()
        {
        //Given
            var encrypter = new Encrypter();
            string password = "Secret1.";

        //When
            Action act = () => encrypter.GetHash(password, "");

        //Then
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void get_hash_method_returns_hash_that_is_different_than_password_and_salt()
        {
        //Given
            var encrypter = new Encrypter();

            string password = "Secret1.";
            string salt = encrypter.GetSalt();

        //When
            var hash = encrypter.GetHash(password, salt);

        //Then
            hash.Should().NotBe(password);
            hash.Should().NotBe(salt);
        }

        [Fact]
        public void given_same_password_and_salt_to_GetHash_retuns_same_values()
        {
        //Given
            var encrypter = new Encrypter();

            string password = "Secret1.";
            string salt = encrypter.GetSalt();

        //When
            var hash1 = encrypter.GetHash(password, salt);
            var hash2 = encrypter.GetHash(password, salt);

        //Then
            hash1.Should().BeEquivalentTo(hash2);
        }

        [Fact]
        public void given_different_passwords_to_GetHash_retuns_different_hashes()
        {
        //Given
            var encrypter = new Encrypter();

            string password1 = "Secret1.";
            string password2 = "Secret2.";
            string salt = encrypter.GetSalt();

        //When
            var hash1 = encrypter.GetHash(password1, salt);
            var hash2 = encrypter.GetHash(password2, salt);

        //Then
            hash1.Should().NotBe(hash2);
        }
        
        [Fact]
        public void given_different_salts_to_GetHash_retuns_different_hashes()
        {
        //Given
            var encrypter = new Encrypter();

            string password = "Secret1.";
            string salt1 = encrypter.GetSalt();
            string salt2 = encrypter.GetSalt();

        //When
            var hash1 = encrypter.GetHash(password, salt1);
            var hash2 = encrypter.GetHash(password, salt2);

        //Then
            hash1.Should().NotBe(hash2);
        }
    }
}