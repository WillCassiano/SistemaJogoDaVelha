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

            sala.Jogadores.Add(new Jogador()
            {
                Id = Context.ConnectionId,
                Nome = usuarioLogados[Context.ConnectionId]
            });

            await Groups.AddToGroupAsync(Context.ConnectionId, sala.Id.ToString());

            await Clients.All.SendAsync("SalaAlterada", sala);

            await Clients.Group(sala.Id.ToString()).SendAsync("EntrouSala", sala);
        }
    }
}
