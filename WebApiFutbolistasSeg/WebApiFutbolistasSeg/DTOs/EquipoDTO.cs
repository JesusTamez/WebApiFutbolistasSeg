namespace WebApiFutbolistasSeg.DTOs
{
    public class EquipoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }
        public List<LigaDTO> Ligas { get; set; }
        
    }
}
