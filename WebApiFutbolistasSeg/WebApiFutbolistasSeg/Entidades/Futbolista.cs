using System.ComponentModel.DataAnnotations;
using WebApiFutbolistasSeg.Validaciones;

namespace WebApiFutbolistasSeg.Entidades
{
    public class Futbolista
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] //
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} solo puede tener hasta 150 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }


        public List<Equipo> equipos { get; set; }
    }
}
