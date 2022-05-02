namespace WebApiFutbolistasSeg.Entidades
{
    public class FutbolistaEquipo
    {
        public int FutbolistaId { get; set; }

        public int EquipoId { get; set; }

        public int Orden { get; set; }

        public Futbolista Futbolista { get; set; }

        public Equipo Equipo { get; set; }
        
    }
}
