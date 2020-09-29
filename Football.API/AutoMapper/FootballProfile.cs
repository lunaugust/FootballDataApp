using AutoMapper;
using Football.Client.Models;
using Football.Domain;

namespace Football.API.AutoMapper
{
    public class FootballProfile : Profile
    {
        public FootballProfile()
        {
            this.CreateMap<Competition, CompetitionModel>()
            .ReverseMap()
            .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name));

            this.CreateMap<Team, TeamModel>()
              .ReverseMap()
              .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
              .ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.Squad));

            this.CreateMap<Player, PlayerModel>()
              .ReverseMap();
        }
    }
}
