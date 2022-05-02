using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFutbolistasSeg.DTOs;
using WebApiFutbolistasSeg.Entidades;


namespace WebApiFutbolistasSeg.Controllers
{
    [ApiController]
    [Route("equipos/{equipoId:int}/ligas")]
    public class LigasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public LigasController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<LigaDTO>>> Get(int equipoId)
        {
            var existeEquipo = await dbContext.Equipos.AnyAsync(equipoDB => equipoDB.Id == equipoId);
            if (!existeEquipo)
            {
                return NotFound();
            }
            var ligas = await dbContext.Ligas.Where(ligasDB => ligasDB.EquipoId == equipoId).ToListAsync();

            return mapper.Map<List<LigaDTO>>(ligas);
        }

        [HttpGet("{id:int}", Name = "obtenerLiga")]
        public async Task<ActionResult<LigaDTO>> GetById(int id)
        {
            var liga = await dbContext.Ligas.FirstOrDefaultAsync(ligaDB => ligaDB.Id == id);

            if(liga == null)
            {
                return NotFound();
            }

            return mapper.Map<LigaDTO>(liga);
        }
        [HttpPost]
        public async Task<ActionResult> Post(int equipoId, LigaCreacionDTO ligaCreacionDTO)
        {
            var existeEquipo = await dbContext.Equipos.AnyAsync(equipoDB => equipoDB.Id == equipoId);
            if (!existeEquipo)
            {
                return NotFound();
            }

            var liga = mapper.Map<Ligas>(ligaCreacionDTO);
            liga.EquipoId = equipoId;
            dbContext.Add(liga);
            await dbContext.SaveChangesAsync();

            var ligaDTO = mapper.Map<LigaDTO>(liga);

            return CreatedAtRoute("obtenerLiga",new {id =liga.Id, equipoId = equipoId}, ligaDTO);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int equipoId,int id, LigaCreacionDTO ligaCreacionDTO)
        {
            var existeEquipo = await dbContext.Equipos.AnyAsync(equipoDB => equipoDB.Id == equipoId);
            if (!existeEquipo)
            {
                return NotFound();
            }
            var existeLiga = await dbContext.Ligas.AnyAsync(ligaDB => ligaDB.Id == id);
            if (!existeLiga)
            {
                return NotFound();
            }

            var liga = mapper.Map<Ligas>(ligaCreacionDTO);
            liga.Id = id;
            liga.EquipoId = equipoId;

            dbContext.Update(liga);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
