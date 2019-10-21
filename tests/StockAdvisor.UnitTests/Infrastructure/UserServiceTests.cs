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
            _passwordHash = "somePasswordHashValue";

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
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email)).Returns(Task.FromResult(user));

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<User, UserDto>(It.IsAny<User>()))
                      .Returns(expectedUserDto);

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

            //When
            var userDto = await userService.GetAsync(_email);

            //Then
            userDto.Should().BeEquivalentTo(expectedUserDto, options =>
                options.ExcludingNestedObjects());
            mapperMock.Verify(x => x.Map<User, UserDto>(It.IsAny<User>()), Times.Once);
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
            Assert.Equal(_role.ToString(), registeredUser.Role);
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
            await Assert.ThrowsAsync<ServiceException>(act);
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
            await Assert.ThrowsAsync<ServiceException>(act);
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
            await Assert.ThrowsAsync<ServiceException>(act);
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
                x.Map<IEnumerable<User>, IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()),
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
                x.Map<IEnumerable<User>, IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                      .Returns(users);

            var encrypterMock = new Mock<IEncrypter>();

            var userService = new UserService(userRepositoryMock.Object,
                encrypterMock.Object, mapperMock.Object);

            //When
            var returnedUsers = await userService.BrowseAsync();
            
            //Then
            returnedUsers.Should().BeSameAs(users);
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