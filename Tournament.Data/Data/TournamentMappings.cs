using AutoMapper;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings()
    {
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<TournamentDetails, TournamentDto>().ReverseMap();
    }
}