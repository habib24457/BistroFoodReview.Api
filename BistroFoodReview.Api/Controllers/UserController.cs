using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;
using BistroFoodReview.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BistroFoodReview.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserRepository userRepository,IMapper mapper):ControllerBase
{
    [HttpGet("allUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userRepository.GetAllUsersAsync();
        return Ok(mapper.Map<List<UserDto>>(users));
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody]CreateUserDto createUserDto)
    {
        var userDomain = mapper.Map<User>(createUserDto);
        userDomain.Id = new Guid();
        var createUser = await userRepository.AddUserAsync(userDomain);
        var userDto = mapper.Map<UserDto>(createUser);
        return CreatedAtAction(nameof(CreateUser), new { id = userDto.Id }, userDto);
    }

    [HttpGet("userWithRatings/{id}")]
    public async Task<IActionResult> GetUsersWithRatingsById(Guid id)
    {
        if (id == null)
            return BadRequest();
        var users = await userRepository.GetUserByIdAsync(id);
        if (users == null)
            return NotFound();
        var usersWithRatingsDto = mapper.Map<UserWithRatingsDto>(users);
        return Ok(usersWithRatingsDto);
    }

    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var deletedUser = await userRepository.DeleteUserAsync(id);
        if (deletedUser == null)
            return NotFound($"User with id {id} not found.");

        var userDto = mapper.Map<UserDto>(deletedUser);
        return Ok(userDto);
    }

}