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
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "nuevosRegistros.txt";
        private readonly string nombreArchivo2 = "registroConsultado.txt";
        private Timer timer;

        public FutbolistasController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.env = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetFutbolistaDTO>>> Get()
        {
            var futbolistas = await dbContext.Futbolistas.ToListAsync();
            return mapper.Map<List<GetFutbolistaDTO>>(futbolistas);
        }


    [HttpGet("{id:int}")]
        public async Task<ActionResult<GetFutbolistaDTO>> Get(int id)
        {
            var futbolista = await dbContext.Futbolistas.FirstOrDefaultAsync(futbolistaBD => futbolistaBD.Id == id);
            {
                return NotFound();
            }

            return mapper.Map<GetFutbolistaDTO>(futbolista);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetFutbolistaDTO>>> Get([FromRoute] string nombre)
        {
            var futbolistas = await dbContext.Futbolistas.Where(futbolistaBD => futbolistaBD.Nombre.Contains(nombre)).ToListAsync();
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo2}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            { writer.WriteLine($@"Datos Consultados: {nombre}"); }
            return mapper.Map<List<GetFutbolistaDTO>>(futbolistas);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]FutbolistaDTO futbolistaDTO)
        {
            var existeFutbolistaMismoNombre = await dbContext.Futbolistas.AnyAsync(x => x.Nombre == futbolistaDTO.Nombre);

            if (existeFutbolistaMismoNombre)
            {
                return BadRequest($"Ya existe un futbolista con el nombre {futbolistaDTO.Nombre}");
            }

            var futbolista = mapper.Map<Futbolista>(futbolistaDTO);
            dbContext.Add(futbolista);
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            { writer.WriteLine($@"Datos Ingresados: {futbolista.Id},{futbolista.Nombre}"); }
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Futbolista futbolista, int id)
        {
            var exist = await dbContext.Futbolistas.AnyAsync(x =>x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if(futbolista.Id != id)
            {
                return BadRequest("El id del futbolista no coincide con el establecido en la url. ");
            }

            dbContext.Update(futbolista);
            await dbContext.SaveChangesAsync();
            return Ok();
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
