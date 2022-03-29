using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFutbolistasSeg.Entidades;

namespace WebApiFutbolistasSeg.Controllers
{
    [ApiController]
    [Route("equipos")]
    public class EquiposController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EquiposController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        [HttpGet("/listadoEquipo")]
        public async Task<ActionResult<List<Equipo>>> GetAll()
        {
            return await dbContext.Equipos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Equipo>> GetById(int id)
        {
            return await dbContext.Equipos.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Equipo equipo)
        {
            //var existeFutbolista = await dbContext.Futbolistas.AnyAsync(x => x.Id == equipo.FutbolistaId);
            //if (!existeFutbolista)
            //{
            //   return BadRequest($"No existe el futbolista con el id: {equipo.FutbolistaId}");
            //}

            dbContext.Add(equipo);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Equipo equipo, int id)
        {
            var exist = await dbContext.Equipos.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("El equipo especificado no existe. ");
            }

            if(equipo.Id != id)
            {
                return BadRequest("El id del equipo no coincide con el establecido en la url. ");
            }

            dbContext.Update(equipo);
            await dbContext.SaveChangesAsync();
            return Ok();
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
    }
}
