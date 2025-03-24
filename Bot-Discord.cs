using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

public class BotDiscord
{
    private string _tokenDiscord;
    private string _idServidor;
    private string _idCategoria;
    private DiscordClient? _clienteDiscord;

    public BotDiscord(string tokenDiscord, string idServidor, string idCategoria)
    {
        _tokenDiscord = tokenDiscord;
        _idServidor = idServidor;
        _idCategoria = idCategoria;
    }

    public async Task IniciarAsync()
    {
        var configuracionDiscord = new DiscordConfiguration
        {
            Token = _tokenDiscord,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All
        };

        _clienteDiscord = new DiscordClient(configuracionDiscord);
        await _clienteDiscord.ConnectAsync();
        Console.WriteLine("Conectado a Discord.");

        var canal = await CrearCanalDiscord();
        if (canal == null)
        {
            Console.WriteLine("Error al crear el canal en Discord.");
            return;
        }
        Console.WriteLine("Canal creado en Discord.");
    }

    private async Task<DiscordChannel?> CrearCanalDiscord()
    {
        if (_clienteDiscord == null)
        {
            Console.WriteLine("Error: El cliente de Discord no está inicializado.");
            return null;
        }

        var servidor = await _clienteDiscord.GetGuildAsync(ulong.Parse(_idServidor));

        if (servidor == null)
        {
            Console.WriteLine($"Error: Servidor con ID {_idServidor} no encontrado.");
            return null;
        }

        var categoria = servidor.Channels.GetValueOrDefault(ulong.Parse(_idCategoria));
        if (categoria == null)
        {
            Console.WriteLine("Error: Categoría no encontrada.");
            return null;
        }

        var canal = await servidor.CreateTextChannelAsync($"Logs-{Dns.GetHostName()}", categoria);
        return canal;
    }
}
