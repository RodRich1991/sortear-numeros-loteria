using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loterias
{
    public class Jogo
    {
        public string nomeLoteria { get; set; }
        public int tentativas { get; set; }
        public int[] numerosSorteio { get; set; }
        public int[] numerosJogados { get; set; }
        public int acertos { get; set; }

        public Jogo(string nomeLoteria, int tentativas, int[] numerosSorteio, int[] numerosJogados)
        {
            this.nomeLoteria = nomeLoteria;
            this.tentativas = tentativas;
            this.numerosSorteio = numerosSorteio;
            this.numerosJogados = numerosJogados;
            acertos = numerosSorteio.Intersect(numerosJogados).Count();
        }

        public override string ToString()
        {
            return "Jogo: " + nomeLoteria +
                "\nTentativas: " + tentativas +
                "\nNumeros Sorteio: " + string.Join(",", numerosSorteio)+
                "\nNumeros Jogados: " + string.Join(",", numerosJogados)+
                "\nAcertos: " + acertos +
                "\n";
        }
    }

    public class TipoJogo
    {
        public string nome { get; set; }
        public int numeroInicial { get; set; }
        public int numeroFinal { get; set; }
        public int qtdeNumerosMarcar { get; set; }
        public int qtdeNumeroAcertar { get; set; }
    }

    class Program
    {

        private static Random random = new Random();
        private static List<Jogo> jogos = new List<Jogo>();
        private static List<TipoJogo> loterias = new List<TipoJogo>
        {
            new TipoJogo {  nome = "Mega Sena", numeroInicial = 1, numeroFinal = 60, qtdeNumeroAcertar = 6, qtdeNumerosMarcar = 6 },
            new TipoJogo { nome = "Loto Fácil", numeroInicial = 1, numeroFinal = 25, qtdeNumeroAcertar = 15, qtdeNumerosMarcar = 15 }
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Seja Bem-vindo!!");
            MenuPrincipal();
        }

        public static void MenuPrincipal()
        {
            int opcao = 0;
            do
            {
                Console.WriteLine("\nDigite o número correspondente ao jogo que quer fazer:");
                for (int i = 0; i < loterias.Count; i++)
                {
                    Console.WriteLine((i + 1) + " - " + loterias.ElementAt(i).nome);
                }
                Console.WriteLine((loterias.Count + 1) + " - Sair\n");

                opcao = Convert.ToInt32(Console.ReadLine());

                if (opcao <= loterias.Count)
                {
                    MenuJogo(loterias.ElementAt(opcao - 1));
                }

            } while (opcao > 0 && opcao <= loterias.Count);
        }

        public static void MenuJogo(TipoJogo tipoJogo)
        {
            Console.WriteLine("Digite a quantidade de jogos que quer fazer:\n");
            int qtd = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < qtd; i++)
                GerarGanhador(SortearNumeros(tipoJogo), tipoJogo);

            Console.WriteLine("Prontinho... aqui os seus " + qtd + "x jogos...");
            jogos.ForEach(j => Console.WriteLine(j.ToString()));
            File.WriteAllLines(@"D:\Área de Trabalho\JogosLoteria.txt", jogos.OrderBy(j => j.tentativas).Select(j => j.ToString()));
        }

        public static int[] SortearNumeros(TipoJogo tipoJogo)
        {
            List<int> numeros = new List<int>();
            do
            {
                int numero = random.Next(tipoJogo.numeroInicial, tipoJogo.numeroFinal + 1);
                if (!numeros.Contains(numero))
                {
                    numeros.Add(numero);
                }
            } while (numeros.Count < tipoJogo.qtdeNumerosMarcar);
            return numeros.ToArray();
        }

        public static bool VerificaSeAcertouNumeros(int[] numerosSorteados, int[] numerosJogados)
        {
            return numerosSorteados.All(n => numerosJogados.Contains(n));
        }

        public static void GerarGanhador(int[] numerosSorteio, TipoJogo tipoJogo)
        {
            int tentativas = 0;
            int[] tentiva;
            string jogoAtual = string.Join(",", numerosSorteio);
            Console.WriteLine("Sorteio atual: ", jogoAtual);
            do
            {
                tentiva = SortearNumeros(tipoJogo);
                tentativas += 1;
                Jogo jogo = new Jogo(tipoJogo.nome, tentativas, numerosSorteio, tentiva);
                if (jogo.acertos == (tipoJogo.qtdeNumeroAcertar - 1))
                {
                    Console.WriteLine("Quase!!\n" + jogo.ToString());
                }
            } while (!VerificaSeAcertouNumeros(numerosSorteio, tentiva));
            Console.WriteLine("\n\nParabéns, Acertou!!");
            jogos.Add(new Jogo(tipoJogo.nome, tentativas, numerosSorteio, tentiva));
        }
    }
}
