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

        public List<Casa> CasasClicadas { get; set; }

        public string JogadorDaVez { get; set; }

        public string GanhadorRodada { get; set; }

        public string GanhadorRodadaNome
        {
            get
            {
                if (GanhadorRodada == "X")
                {
                    return Jogadores[0].Nome;
                }
                else if (GanhadorRodada == "O")
                {
                    return Jogadores[1].Nome;
                }
                else
                {
                    return "";
                }
            }
        }

        public bool Fechada { get { return Jogadores.Count >= 2; } }

        public bool TemGanhador
        {
            get
            {
                return VerificaGanhadores();
            }
        }


        public List<int[]> Combinacoes = new List<int[]>()
        {
            /* Horizontais */
            new int[] { 1, 2, 3},
            new int[] { 4, 5, 6},
            new int[] { 7, 8, 9},

            /* Verticais */
            new int[] { 1, 4, 7},
            new int[] { 2, 5, 8},
            new int[] { 3, 6, 9},

            /* Diagonais */
            new int[] { 1, 5, 9},
            new int[] { 3, 5, 7}
        };

        private bool VerificaGanhadores()
        {
            GanhadorRodada = "";

            foreach (var combinacao in Combinacoes)
            {
                var jogadorX = CasasClicadas.FindAll(p => p.Jogador == "X" && combinacao.Contains(p.Numero));
                var jogadorO = CasasClicadas.FindAll(p => p.Jogador == "O" && combinacao.Contains(p.Numero));

                if (jogadorX.Count == 3)
                {
                    GanhadorRodada = "X";
                }
                if (jogadorO.Count == 3)
                {
                    GanhadorRodada = "O";
                }
                if (GanhadorRodada != "")
                {
                    return true;
                }
            }

            return false;
        }

        public Sala()
        {
            Jogadores = new List<Jogador>();
            CasasClicadas = new List<Casa>();
            JogadorDaVez = "X";
        }
    }
}
