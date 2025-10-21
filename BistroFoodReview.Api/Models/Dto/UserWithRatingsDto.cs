namespace BistroFoodReview.Api.Models.Dto;

public class UserWithRatingsDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<RatingDto> Ratings { get; set; } = new(); 
}