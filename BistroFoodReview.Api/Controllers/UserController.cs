using AutoMapper;
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

}