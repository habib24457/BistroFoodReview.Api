using AutoMapper;
using BistroFoodReview.Api.Controllers;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace BistroFoodReview.Api.Test.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var userRepository = Substitute.For<IUserRepository>();
        var mapper = Substitute.For<IMapper>();
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User 1" },
            new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User 2" }
        };

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName
        }).ToList();

        userRepository.GetAllUsersAsync().Returns(users);
        mapper.Map<List<UserDto>>(users).Returns(userDtos);

        var controller = new UserController(userRepository, mapper);

        // Act
        var result = await controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
    }
    
    [Fact]
    public async Task GetUsersWithRatingsById_ShouldReturnUserWithRatings()
    {
        // Arrange
        var userRepository = Substitute.For<IUserRepository>();
        var mapper = Substitute.For<IMapper>();

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Ratings = new List<Rating>
            {
                new Rating { Id = Guid.NewGuid(), Stars = 4 },
                new Rating { Id = Guid.NewGuid(), Stars = 5 }
            }
        };

        var userWithRatingsDto = new UserWithRatingsDto
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Ratings = new List<RatingDto>
            {
                new RatingDto { Stars = 4 },
                new RatingDto { Stars = 5 }
            }
        };

        userRepository.GetUserByIdAsync(userId).Returns(user);
        mapper.Map<UserWithRatingsDto>(user).Returns(userWithRatingsDto);

        var controller = new UserController(userRepository, mapper);

        // Act
        var result = await controller.GetUsersWithRatingsById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserWithRatingsDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
        Assert.Equal("Test", returnedUser.FirstName);
        Assert.Equal(2, returnedUser.Ratings.Count);
    }

}