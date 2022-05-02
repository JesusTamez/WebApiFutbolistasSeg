using System.ComponentModel.DataAnnotations;
using WebApiFutbolistasSeg.Validaciones;

namespace WebApiFutbolistasSeg.DTOs
{
    public class EquipoCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        public List<int> FutbolistasIds { get; set; }
    }
}
