using System;
using System.Threading.Tasks;
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
            _salt = "salt";
        readonly Guid _id = Guid.NewGuid();

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
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(_email)).Returns(Task.FromResult(user));
            var userService = new UserService(userRepository.Object);

            //When
            var userDto = await userService.GetAsync(_email);

            //Then
            expectedUserDto.Should().BeEquivalentTo(userDto, options =>
                options.ExcludingNestedObjects());
        }
        [Fact]

        public async Task get_async_throws_service_exception_if_user_does_not_exists()
        {
            //Given
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAsync(_email))
                          .Returns(Task.FromResult((User)null));
            
            var userService = new UserService(userRepository.Object);

            //When
            Func<Task<UserDto>> act = () => userService.GetAsync(_email);

            //Then
            await Assert.ThrowsAsync<ServiceException>(act);
        }

        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
            //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            
            var userService = new UserService(userRepositoryMock.Object);
            await userService.RegisterAsync(_email, _firstname,
                _surname, _password);

            //When 
            //Then
            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task register_async_throws_exception_if_email_is_in_use()
        {
            //Given
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAsync(_email))
                              .Returns(Task.FromResult(GetDefaultUser()));
            
            var userService = new UserService(userRepositoryMock.Object);

            //When 
            Func<Task> act = () => userService.RegisterAsync(_email, _firstname, _surname, _password); 

            // Then
            await Assert.ThrowsAsync<ServiceException>(act);
        }

        private User GetDefaultUser()
            => new User(_id, _email, _firstname, _surname, _password, _salt);
    }
}