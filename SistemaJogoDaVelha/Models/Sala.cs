using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaJogoDaVelha.Models
{
    public class Sala
    {
        public int Id { get; set; }
        public List<Jogador> Jogadores { get; set; }
        public bool Fechada { get { return Jogadores.Count >= 2; } }

        public Sala()
        {
            Jogadores = new List<Jogador>();
        }
    }
}
