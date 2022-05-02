namespace WebApiFutbolistasSeg.Entidades
{
    public class Ligas
    {
        public int Id {get; set; }

        public string Campeonato { get; set; }

        public int EquipoId { get; set; }

        public Equipo Equipo { get; set; }
    }
}
