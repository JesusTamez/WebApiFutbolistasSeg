using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFutbolistasSeg.DTOs;
using WebApiFutbolistasSeg.Entidades;


namespace WebApiFutbolistasSeg.Controllers
{
    [ApiController]
    [Route("futbolistas")]
    public class FutbolistasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public FutbolistasController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
            
        }

        [HttpGet]
        public async Task<ActionResult<List<GetFutbolistaDTO>>> Get()
        {
            var futbolistas = await dbContext.Futbolistas.ToListAsync();
            return mapper.Map<List<GetFutbolistaDTO>>(futbolistas);
        }


        [HttpGet("{id:int}", Name = "obtenerfutbolista")]
        public async Task<ActionResult<FutbolistaDTOConEquipos>> Get(int id)
        {
            var futbolista = await dbContext.Futbolistas
                .Include(futbolistaDB => futbolistaDB.FutbolistaEquipo)
                .ThenInclude(futbolistaEquipoDB => futbolistaEquipoDB.Equipo)
                .FirstOrDefaultAsync(futbolistaDB => futbolistaDB.Id == id);
            
            if(futbolista == null)
            {
                return NotFound();
            }

            return mapper.Map<FutbolistaDTOConEquipos>(futbolista);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetFutbolistaDTO>>> Get([FromRoute] string nombre)
        {
            var futbolistas = await dbContext.Futbolistas.Where(futbolistaBD => futbolistaBD.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<GetFutbolistaDTO>>(futbolistas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]FutbolistaDTO futbolistaDto)
        {
            var existeFutbolistaMismoNombre = await dbContext.Futbolistas.AnyAsync(x => x.Nombre == futbolistaDto.Nombre);

            if (existeFutbolistaMismoNombre)
            {
                return BadRequest($"Ya existe un futbolista con el nombre {futbolistaDto.Nombre}");
            }

            var futbolista = mapper.Map<Futbolista>(futbolistaDto);
            dbContext.Add(futbolista);
            await dbContext.SaveChangesAsync();

            var futbolistaDTO = mapper.Map<GetFutbolistaDTO>(futbolista);
            return CreatedAtRoute("obtenerfutbolista", new {id = futbolista.Id},futbolistaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(FutbolistaDTO futbolistaCreacionDTO, int id)
        {
            var exist = await dbContext.Futbolistas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            var futbolista = mapper.Map<Futbolista>(futbolistaCreacionDTO);
            futbolista.Id = id;

            dbContext.Update(futbolista);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Futbolistas.AnyAsync(x =>x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado. ");
            }

            dbContext.Remove(new Futbolista()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
