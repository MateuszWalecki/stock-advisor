using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using Xunit;

namespace StockAdvisor.UnitTests.Infrastructure
{
    public class UserServiceTests
    {
        private readonly string _email = "email@email.com",
            _firstname = "Leo",
            _surname = "Messi",
            _password = "secret",
            _salt = "someSaltValue",
            _passwordHash = "somePasswordHashValue",
            _validPassword = "SomeValidPassword12.";

        private readonly Guid _id = Guid.NewGuid();
        private readonly UserRole _role = UserRole.User;

        [Fact]
        public async Task get_async_should_invoke_get_async_on_repository()
        {
        //Given
            var user = GetDefaultUser();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email))
                              .Returns(Task.FromResult(user));

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);
            await userService.GetAsync(_email);

        //When 
        //Then
            userRepositoryMock.Verify(x => x.GetAsync(_email), Times.Once);
        }

        [Fact]
        public async Task get_async_returns_mapped_user_if_exists()
        {
        //Given
            User user = GetDefaultUser();
            UserDto expectedUserDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                SurName = user.SurName,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email)).Returns(Task.FromResult(user));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
                      .Returns(expectedUserDto);

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            var userDto = await userService.GetAsync(_email);

        //Then
            userDto.Should().BeEquivalentTo(expectedUserDto, options =>
                options.ExcludingNestedObjects());
            mapperMock.Verify(x => x.Map<UserDto>(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task get_async_returns_null_if_user_does_not_exists()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email))
                          .Returns(Task.FromResult((User)null));
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            var user = await userService.GetAsync(_email);

        //Then
            user.Should().BeNull();
        }

        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
        //Given
            User registeredUser = null;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
                              .Callback<User>(u => registeredUser = u)
                              .Returns(Task.CompletedTask);     
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = GetConfiguredEncrypter();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);       


        //When 
            await userService.RegisterAsync(_id, _email, _firstname,
                _surname, _password, _role);

        //Then
            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            registeredUser.Should().NotBeNull();
            Assert.Equal(_id, registeredUser.Id);
            Assert.Equal(_email, registeredUser.Email);
            Assert.Equal(_firstname, registeredUser.FirstName);
            Assert.Equal(_surname, registeredUser.SurName);
            Assert.Equal(_role, registeredUser.Role);
        }

        [Fact]
        public async Task register_async_should_invoke_GetSalt_and_GetHash_on_enrypter()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = GetConfiguredEncrypter();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When 
            await userService.RegisterAsync(_id, _email, _firstname,
                _surname, _password, _role);

        //Then
            encrypterMock.Verify(x => x.GetSalt(), Times.Once);
            encrypterMock.Verify(x => x.GetHash(_password, _salt), Times.Once);
        }

        [Fact]
        public async Task register_async_throws_exception_if_email_is_in_use()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email))
                              .Returns(Task.FromResult(GetDefaultUser()));
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When 
            Func<Task> act = () => userService.RegisterAsync(_id, _email, _firstname, _surname,
                _password, _role); 

            // Then
            await Assert.ThrowsAsync<EmailInUseSerExc>(act);
        }

        [Fact]
        public async Task register_async_throws_exception_if_email_is_invalid()
        {
        //Given
            string unvalidEmail = "";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(unvalidEmail))
                              .ReturnsAsync((User)null);
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When 
            Func<Task> act = () => userService.RegisterAsync(_id, unvalidEmail, _firstname, _surname,
                _password, _role); 

            // Then
            await Assert.ThrowsAsync<InvalidEmailSerExc>(act);
        }

        [Fact]
        public async Task loginasync_throws_exception_if_repo_getasync_returns_null()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult((User)null));
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.LoginAsync(_email, _password);
            
        //Then
            await Assert.ThrowsAsync<InvalidCredentialsSerExc>(act);
        }

        [Fact]
        public async Task loginasync_calls_encrypter_gethash_method()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(GetDefaultUser()));
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(_passwordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            await userService.LoginAsync(_email, _password);
            
        //Then
            encrypterMock.Verify(x => x.GetHash(It.IsAny<string>(),
                                                It.IsAny<string>()),
                                 Times.Once);
        }

        [Fact]
        public async Task if_password_is_incorrect_exception_is_thrown()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                              .Returns(Task.FromResult(GetDefaultUser()));
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns("differntHash");

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.LoginAsync(_email, _password);
            
        //Then
            await Assert.ThrowsAsync<InvalidCredentialsSerExc>(act);
        }

        [Fact]
        public async Task browse_async_invokes_browseasync_on_repo()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            var returnedUsers = await userService.BrowseAsync();
            
        //Then
            userRepositoryMock.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task browse_async_invokes_map_on_mapper()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            var returnedUsers = await userService.BrowseAsync();
            
        //Then
            mapperMock.Verify(x =>
                x.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()),
                                            Times.Once);
        }
        
        [Fact]
        public async Task browse_async_returns_mapping_result()
        {
        //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            IEnumerable<UserDto> users = new List<UserDto>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x =>
                x.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                      .Returns(users);

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            var returnedUsers = await userService.BrowseAsync();
            
        //Then
            returnedUsers.Should().BeSameAs(users);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        public async Task change_password_throws_exception_if_new_password_is_invalid(string newPassword)
        {
        //Given
            var user = GetDefaultUser();

            var userRepositoryMock = new Mock<IUserRepository>();

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserPasswordAsync(user.Id, newPassword,
                It.IsAny<string>());
        
        //Then
            await Assert.ThrowsAsync<InvalidPasswordSerExc>(act);
        }

        [Fact]
        public async Task change_password_throws_exception_if_old_password_does_not_match()
        {
        //Given
            var user = GetDefaultUser();
            string newPassword = "newasfasgf3254.,34,5.";
            string oldPassword = "dsagagt34.tsag.e";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(oldPassword, user.Salt))
                         .Returns(user.PasswordHash + "dsa");

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserPasswordAsync(user.Id, newPassword,
                oldPassword);
        
        //Then
            await Assert.ThrowsAsync<InvalidCredentialsSerExc>(act);
        }
        [Fact]
        public async Task change_password_throws_exception_if_given_userid_is_invalid()
        {
        //Given
            var user = GetDefaultUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync((User)null);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserPasswordAsync(user.Id, _validPassword,
                It.IsAny<string>());
        
        //Then
            await Assert.ThrowsAsync<UserNotFoundSerExc>(act);
        }

        [Fact]
        public async Task change_password_throws_exception_if_new_password_equals_current()
        {
        //Given
            var user = GetDefaultUser();
            string oldAndNewPassword = _validPassword;
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(oldAndNewPassword, user.Salt))
                         .Returns(user.PasswordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserPasswordAsync(user.Id, oldAndNewPassword,
                oldAndNewPassword);
        
        //Then
            await Assert.ThrowsAsync<InvalidPasswordSerExc>(act);
        }

        [Fact]
        public async Task on_succes_changing_password_set_password_method_is_called_with_encrypter_outputs()
        {
        //Given
            var user = GetDefaultUser();
            var oldUpdatedAt = user.UpdatedAt;

            string oldPassword = _validPassword;
            string newPassword = _validPassword + "sadasd";

            string newSalt = "newSalt";
            string newHash = "newHash";
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(oldPassword, user.Salt))
                         .Returns(user.PasswordHash);
            encrypterMock.Setup(x => x.GetSalt())
                         .Returns(newSalt);
            encrypterMock.Setup(x => x.GetHash(newPassword, newSalt))
                         .Returns(newHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            await userService.ChangeUserPasswordAsync(user.Id, newPassword, oldPassword);
        
        //Then
            encrypterMock.Verify(x => x.GetSalt(), Times.Once);
            encrypterMock.Verify(x => x.GetHash(newPassword, newSalt), Times.Once);

            user.UpdatedAt.Should().NotBe(oldUpdatedAt);
            user.Salt.Should().Be(newSalt);
            user.PasswordHash.Should().Be(newHash);

            userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task change_password_throws_exception_if_coupled_user_cannot_be_found()
        {
        //Given
            var user = GetDefaultUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync((User)null);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserEmailAsync(user.Id, It.IsAny<string>(),
                It.IsAny<string>());
        
        //Then
            await Assert.ThrowsAsync<UserNotFoundSerExc>(act);
        }

        [Fact]
        public async Task change_email_throws_exception_when_related_used_cannot_be_found()
        {
        //Given
            var user = GetDefaultUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync((User)null);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserEmailAsync(user.Id, It.IsAny<string>(),
                It.IsAny<string>());
        
        //Then
            await Assert.ThrowsAsync<UserNotFoundSerExc>(act);
        }

        [Fact]
        public async Task change_email_throws_exception_when_given_password_is_invalid()
        {
        //Given
            string password = "SecretPassword";
            string newEmail = "new_email@test.com";
            var user = GetDefaultUser();
            var differentPasswordHash = user.PasswordHash + "aa";
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(password, user.Salt))
                         .Returns(differentPasswordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserEmailAsync(user.Id, password,
                newEmail);
        
        //Then
            await Assert.ThrowsAsync<InvalidCredentialsSerExc>(act);
        }

        [Fact]
        public async Task change_email_throws_exception_when_given_email_is_invalid()
        {
        //Given
            string password = "SecretPassword";
            string newEmail = "InvalidEmail";
            var user = GetDefaultUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(password, user.Salt))
                         .Returns(user.PasswordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserEmailAsync(user.Id, password,
                newEmail);
        
        //Then
            await Assert.ThrowsAsync<InvalidEmailSerExc>(act);
        }

        [Fact]
        public async Task change_email_throws_exception_when_given_email_equals_current()
        {
        //Given
            string password = "SecretPassword";
            var user = GetDefaultUser();
            
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(password, user.Salt))
                         .Returns(user.PasswordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => userService.ChangeUserEmailAsync(user.Id, password,
                user.Email);
        
        //Then
            await Assert.ThrowsAsync<InvalidEmailSerExc>(act);
        }

        [Fact]
        public async Task change_email_sets_new_email_and_updates_repo_on_success()
        {
        //Given
            string password = "SecretPassword";
            string newEmail = "new_email@test.com";
            var user = GetDefaultUser();
            var originalUpdatedAt = user.UpdatedAt;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(user.Id))
                              .ReturnsAsync(user);

            var mapperMock = new Mock<IMapper>();

            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetHash(password, user.Salt))
                         .Returns(user.PasswordHash);

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

        //When
            await userService.ChangeUserEmailAsync(user.Id, password, newEmail);
        
        //Then
            user.Email.Should().BeEquivalentTo(newEmail);
            user.UpdatedAt.Should().NotBe(originalUpdatedAt);
            userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        private User GetDefaultUser()
            => new User(_id, _email, _firstname, _surname, _passwordHash, _salt, _role);

        private Mock<IEncrypter> GetConfiguredEncrypter()
        {
            var encrypterMock = new Mock<IEncrypter>();
            encrypterMock.Setup(x => x.GetSalt())
                         .Returns(_salt);
            encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                         .Returns(_passwordHash);

                         return encrypterMock;
        }
    }
}