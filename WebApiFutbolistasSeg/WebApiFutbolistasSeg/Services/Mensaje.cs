namespace WebApiFutbolistasSeg.Services
{
    public class Mensaje : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Punto3.txt";
        private Timer timer;

        public Mensaje(IWebHostEnvironment env)
        {
        this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));
        return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
        timer.Dispose();
        return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
        var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
        using (StreamWriter writer = new StreamWriter(ruta, append: true))
        { writer.WriteLine("El Profe Gustavo Rodriguez es el mejor"); }
        }
    }
}

