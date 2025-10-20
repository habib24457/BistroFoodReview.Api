using AutoMapper;
using BistroFoodReview.Api.Models.Domain;
using BistroFoodReview.Api.Models.Dto;

namespace BistroFoodReview.Api.Mappings;

public class AutoMapperProfiles:Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Meal,MealDto>()
            .ForMember(dest => dest.MealName, opt => opt.MapFrom(src => src.EditedMealName ?? src.MealOption.Name))
            .ForMember(dest => dest.MealOptionName, opt => opt.MapFrom(src => src.MealOption.Name));
    }
}