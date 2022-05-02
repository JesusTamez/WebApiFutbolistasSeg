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
            CreateMap<Futbolista, FutbolistaDTOConEquipos>()
                .ForMember(futbolistaDTO => futbolistaDTO.Equipos, opciones => opciones.MapFrom(MapFutbolistaDTOEquipos));
            CreateMap<EquipoCreacionDTO, Equipo>()
                .ForMember(equipo => equipo.FutbolistaEquipo, opciones => opciones.MapFrom(MapFutbolistaEquipo));
            CreateMap<Equipo, EquipoDTO>();
            CreateMap<Equipo, EquipoDTOConFutbolistas>()
                .ForMember(equipoDTO => equipoDTO.Futbolistas, opciones => opciones.MapFrom(MapEquipoDTOFutbolistas));
            CreateMap<EquipoPatchDTO, Equipo>().ReverseMap();
            CreateMap<LigaCreacionDTO, Ligas>();
            CreateMap<Ligas, LigaDTO>();
        }

        private List<EquipoDTO> MapFutbolistaDTOEquipos(Futbolista futbolista, GetFutbolistaDTO getFutbolistaDTO)
        {
            var result = new List<EquipoDTO>();
            if(futbolista.FutbolistaEquipo == null) { return result; }
            foreach(var futbolistaEquipo in futbolista.FutbolistaEquipo)
            {
                result.Add(new EquipoDTO()
                {
                    Id = futbolistaEquipo.EquipoId,
                    Nombre = futbolistaEquipo.Equipo.Nombre
                });
            }
            return result;
        }

        private List<GetFutbolistaDTO> MapEquipoDTOFutbolistas(Equipo equipo, EquipoDTO equipoDTO)
        {
            var result = new List<GetFutbolistaDTO>();

            if(equipo.FutbolistaEquipo == null)
            {
                return result;
            }

            foreach(var futbolistaequipo in equipo.FutbolistaEquipo)
            {
                result.Add(new GetFutbolistaDTO()
                {
                    Id = futbolistaequipo.FutbolistaId,
                    Nombre = futbolistaequipo.Futbolista.Nombre
                });
            }
            return result;
        }

        private List<FutbolistaEquipo> MapFutbolistaEquipo(EquipoCreacionDTO equipoCreacionDTO, Equipo equipo)
        {
            var resultado = new List<FutbolistaEquipo>();

            if(equipoCreacionDTO.FutbolistasIds == null) { return resultado; }
            foreach(var futbolistaId in equipoCreacionDTO.FutbolistasIds)
            {
                resultado.Add(new FutbolistaEquipo() { FutbolistaId = futbolistaId });
            }

            return resultado;
        }
    }
}
