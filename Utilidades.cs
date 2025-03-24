using System;
using System.IO;
using Newtonsoft.Json;

public static class Configuracion
{
    public static Config CargarConfiguracion()
    {
        try
        {
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Config", "config.json");
            var json = File.ReadAllText(rutaArchivo);
            return JsonConvert.DeserializeObject<Config>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar la configuración: {ex.Message}");
            return new Config();
        }
    }
}

public class Config
{
    public string TokenDiscord { get; set; }
    public string IdServidor { get; set; }
    public string IdCategoria { get; set; }
    public int IntervaloEnvio { get; set; }
    public string RutaCarpetaOculta { get; set; }
}
