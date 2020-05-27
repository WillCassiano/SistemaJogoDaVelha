using Microsoft.AspNetCore.SignalR;
using SistemaJogoDaVelha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaJogoDaVelha.Hubs
{
    public class PrincipalHub : Hub
    {
        public static Dictionary<string, string> usuarioLogados = new Dictionary<string, string>();

        public static List<Sala> _salas = new List<Sala>()
        {
            new Sala(){ Id = 1 },
            new Sala(){ Id = 2 },
            new Sala(){ Id = 3 },
            new Sala(){ Id = 4 },
            new Sala(){ Id = 5 },
            new Sala(){ Id = 6 },
            new Sala(){ Id = 7 },
            new Sala(){ Id = 8 },
            new Sala(){ Id = 9 },
            new Sala(){ Id = 10 }
        };

        public async Task EntrarUsuario(string usuario)
        {
            var user = usuarioLogados.Keys.FirstOrDefault(p => p == Context.ConnectionId);

            if (user == null)
                usuarioLogados.Add(Context.ConnectionId, usuario);

            await Clients.All.SendAsync("UsuariosLogados", usuarioLogados.Values);
        }

        public async Task SpamAll()
        {
            await Clients.All.SendAsync("SpamMessage", usuarioLogados[Context.ConnectionId]);
        }

        public async Task SpamOthers()
        {
            await Clients.Others.SendAsync("SpamMessage", usuarioLogados[Context.ConnectionId]);
        }

        public async Task EntrarSala(int id)
        {
            var sala = _salas.FirstOrDefault(s => s.Id == id);
            if (sala.Fechada)
            {
                await Clients.Caller.SendAsync("SalaFechada", sala.Id);
                return;
            }

            var jogador = new Jogador()
            {
                Id = Context.ConnectionId,
                Nome = usuarioLogados[Context.ConnectionId],
                Marcacao = sala.Jogadores.Count > 0 ? "O" : "X"
            };

            sala.Jogadores.Add(jogador);

            await Groups.AddToGroupAsync(Context.ConnectionId, sala.Id.ToString());

            await Clients.All.SendAsync("SalaAlterada", sala);

            await Reiniciar(sala.Id);

            await Clients.Group(sala.Id.ToString()).SendAsync("EntrouSala", sala, jogador);
        }

        public async Task CasaClicada(int salaId, int numero)
        {
            var sala = _salas.FirstOrDefault(sala => sala.Id == salaId);

            var jogador = sala.Jogadores.FirstOrDefault(p => p.Id == Context.ConnectionId);

            sala.CasasClicadas.Add(new Casa() { Jogador = jogador.Marcacao, Numero = numero });

            sala.JogadorDaVez = sala.JogadorDaVez == "X" ? "O" : "X";

            await Clients.Group(sala.Id.ToString()).SendAsync("AlterarVez", sala, numero);

            if (sala.TemGanhador)
            {
                await Clients.Group(sala.Id.ToString()).SendAsync("JogadorGanhou", sala.GanhadorRodada, sala.GanhadorRodadaNome);
            }
        }

        public async Task SpamGroup(int id)
        {
            await Clients.Group(id.ToString()).SendAsync("SpamMessage", usuarioLogados[Context.ConnectionId]);
        }

        public async Task Reiniciar(int id)
        {
            var sala = _salas.FirstOrDefault(p => p.Id == id);

            sala.CasasClicadas = new List<Casa>();
            sala.JogadorDaVez = "X";

            await Clients.Group(sala.Id.ToString()).SendAsync("SalaReiniciada", sala);
        }

        public async Task AdicionarBot(int salaId)
        {
            var sala = _salas.FirstOrDefault(s => s.Id == salaId);

            if (sala.Fechada)
            {
                await Clients.Caller.SendAsync("SalaFechada", sala.Id);
                return;
            }

            var bot = new JogadorBot(sala)
            {
                Id = "BOT" + sala.Id.ToString(),
                Nome = "Bot",
                Marcacao = "O"
            };

            sala.Jogadores.Add(bot);
            await Groups.AddToGroupAsync(Context.ConnectionId, sala.Id.ToString());

            await Clients.All.SendAsync("SalaAlterada", sala);

            await Clients.Group(sala.Id.ToString()).SendAsync("EntrouSala", sala, bot);
        }
    }
}
