using AutoMapper;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<news_application.Models.Category, CategoryDto>()
                .ForMember(destination => destination.Name, option => option.MapFrom(source => source.CategoryName));

            CreateMap<User, UserDTO>();
            CreateMap<User, LoginResponseDto>();

            CreateMap<ExternalSource, ExternalSourceDto>();

            CreateMap<NewsArticle, NewsArticleDto>();
        }
    }
}
