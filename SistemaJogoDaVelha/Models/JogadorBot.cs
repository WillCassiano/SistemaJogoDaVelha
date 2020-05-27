using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaJogoDaVelha.Models
{
    public class JogadorBot : Jogador
    {
        private Sala _sala;
        private int numeroJogado = 0;

        public JogadorBot(Sala sala)
        {
            _sala = sala;
        }

        public int Jogar()
        {
            numeroJogado = 0;

            if (Atacar())
            {
                return numeroJogado;
            }


            if (Defender())
            {
                return numeroJogado;
            }

            var random = new Random();

            while (true)
            {
                var numeroJogar = random.Next(1, 9);
                var numeroJaJogado = _sala.CasasClicadas.FirstOrDefault(p => p.Numero == numeroJogar);
                if (numeroJaJogado == null)
                {
                    return numeroJogado;
                }
            }            
        }

        private bool Defender()
        {
            foreach (var combinacao in _sala.Combinacoes)
            {
                var numerosJogados = _sala.CasasClicadas.FindAll(p => p.Jogador == "X"
                                                                 && combinacao.Contains(p.Numero));
                if (numerosJogados.Count == 2)
                {
                    foreach (var numero in combinacao)
                    {
                        var temNumero = numerosJogados.FirstOrDefault(p => p.Numero == numero);
                        if (temNumero == null)
                        {
                            var casa = _sala.CasasClicadas.FirstOrDefault(p => p.Numero == numero);
                            if (casa == null)
                            {
                                numeroJogado = numero;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Atacar()
        {
            foreach (var combinacao in _sala.Combinacoes)
            {
                var numerosJogados = _sala.CasasClicadas.FindAll(p => p.Jogador == "O" 
                                                                 && combinacao.Contains(p.Numero));
                if (numerosJogados.Count == 2)
                {
                    foreach (var numero in combinacao)
                    {
                        var temNumero = numerosJogados.FirstOrDefault(p => p.Numero == numero);
                        if (temNumero == null)
                        {                            
                            var casa = _sala.CasasClicadas.FirstOrDefault(p => p.Numero == numero);
                            if (casa == null)
                            {
                                numeroJogado = numero;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
