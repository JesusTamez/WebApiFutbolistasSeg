using AutoMapper;
using WebApiFutbolistasSeg.DTOs;
using WebApiFutbolistasSeg.Entidades;

namespace WebApiFutbolistasSeg.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<FutbolistaDTO, Futbolista>();
            CreateMap<Futbolista, GetFutbolistaDTO>();
        }
    }
}
