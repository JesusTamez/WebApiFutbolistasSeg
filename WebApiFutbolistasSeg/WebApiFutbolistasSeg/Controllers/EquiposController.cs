using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFutbolistasSeg.DTOs;
using WebApiFutbolistasSeg.Entidades;

namespace WebApiFutbolistasSeg.Controllers
{
    [ApiController]
    [Route("equipos")]
    public class EquiposController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public EquiposController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpGet("/listadoEquipo")]
        public async Task<ActionResult<List<Equipo>>> GetAll()
        {
            return await dbContext.Equipos.ToListAsync();
        }

        [HttpGet("{id:int}", Name = "obtenerEquipo")]
        public async Task<ActionResult<EquipoDTOConFutbolistas>> GetById(int id)
        {
            var equipo = await dbContext.Equipos
                .Include(equipoDB => equipoDB.FutbolistaEquipo)
                .ThenInclude(futbolistaEquipoDB => futbolistaEquipoDB.Futbolista)
                .Include(ligaDB => ligaDB.Ligas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(equipo == null)
            {
                return NotFound();
            }

            equipo.FutbolistaEquipo = equipo.FutbolistaEquipo.OrderBy(x => x.Orden).ToList();

            return mapper.Map<EquipoDTOConFutbolistas>(equipo);
        }

        [HttpPost]
        public async Task<ActionResult> Post(EquipoCreacionDTO equipoCreacionDTO)
        {
            if(equipoCreacionDTO.FutbolistasIds == null)
            {
                return BadRequest("No se puede crear un equipo sin futbolistas. ");
            }

            var futbolistasIds = await dbContext.Futbolistas
                .Where(futbolistaDB => equipoCreacionDTO.FutbolistasIds.Contains(futbolistaDB.Id)).Select(x => x.Id).ToListAsync();

            if(equipoCreacionDTO.FutbolistasIds.Count != futbolistasIds.Count)
            {
                return BadRequest("No existe uno de los futbolistas enviados");
            }

            var equipo = mapper.Map<Equipo>(equipoCreacionDTO);

            OrdenarPorFutbolistas(equipo);

            dbContext.Add(equipo);
            await dbContext.SaveChangesAsync();

            var equipoDTO = mapper.Map<EquipoDTO>(equipo);
            return CreatedAtRoute("obtenerEquipo",new {id = equipo.Id},equipoDTO);
        }

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(Equipo equipo, int id)
        //{
        //  var exist = await dbContext.Equipos.AnyAsync(x => x.Id == id);

        //if (!exist)
        // {
        //     return NotFound("El equipo especificado no existe. ");
        // }

        //if(equipo.Id != id)
        //    {
        //        return BadRequest("El id del equipo no coincide con el establecido en la url. ");
        //    }

        //    dbContext.Update(equipo);
        //    await dbContext.SaveChangesAsync();
        //    return Ok();
        // }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, EquipoCreacionDTO equipoCreacionDTO)
        {
            var equipoDB = await dbContext.Equipos
                .Include(x => x.FutbolistaEquipo)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(equipoDB == null)
            {
                return NotFound();
            }

            equipoDB = mapper.Map(equipoCreacionDTO, equipoDB);

            OrdenarPorFutbolistas(equipoDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Equipos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Equipo { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        private void OrdenarPorFutbolistas(Equipo equipo)
        {
            if(equipo.FutbolistaEquipo != null)
            {
                for (int i = 0; i < equipo.FutbolistaEquipo.Count; i++)
                {
                    equipo.FutbolistaEquipo[i].Orden = i;
                }
            }
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<EquipoPatchDTO> patchDocument)
        {
            if(patchDocument == null) { return BadRequest(); }

            var equipoDB = await dbContext.Equipos.FirstOrDefaultAsync(x => x.Id == id);

            if(equipoDB == null) { return NotFound(); }

            var equipoDTO = mapper.Map<EquipoPatchDTO>(equipoDB);

            patchDocument.ApplyTo(equipoDTO);

            var isValid = TryValidateModel(equipoDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(equipoDTO, equipoDB);

            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
