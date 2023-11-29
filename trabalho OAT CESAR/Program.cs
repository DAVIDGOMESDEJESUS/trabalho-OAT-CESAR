using System;
using System.Linq;

class Program
{
    static void Main()
    {
        do
        {
            Jogo();
            Console.WriteLine("Digite '-1' para sair ou '0' para uma nova partida:");
        } while (Console.ReadLine() == "0");
    }

    static void Jogo()
    {
        Console.WriteLine("Jogador 1, digite seu nome:");
        string jogador1 = Console.ReadLine();

        Console.WriteLine("Jogador 2, digite seu nome ou pressione Enter para jogar contra o computador:");
        string jogador2 = Console.ReadLine();
        bool contraComputador = string.IsNullOrWhiteSpace(jogador2);
        if (contraComputador)
            jogador2 = "Computador";

        Random rand = new Random();
        int energiaJogador1 = 10, energiaJogador2 = 10;
        int golsJogador1 = 0, golsJogador2 = 0;
        int pontosJogador1 = 0, pontosJogador2 = 0;

        int turnoAtual = rand.Next(1, 3); // 1 para jogador1, 2 para jogador2

        Console.WriteLine($"O jogador {(turnoAtual == 1 ? jogador1 : jogador2)} começará o jogo!");

        while (energiaJogador1 > 0 && energiaJogador2 > 0)
        {
            ExecutarTurno(turnoAtual, jogador1, jogador2, ref golsJogador1, ref golsJogador2, ref pontosJogador1, ref pontosJogador2, ref energiaJogador1, ref energiaJogador2, rand);
            turnoAtual = 3 - turnoAtual; // Alternar entre jogador1 (1) e jogador2 (2)
        }

        MostrarResultadoFinal(jogador1, jogador2, golsJogador1, golsJogador2, pontosJogador1, pontosJogador2);
    }

    static void ExecutarTurno(int currentPlayer, string jogador1, string jogador2, ref int golsJogador, ref int golsAdversario,
        ref int pontosJogador, ref int pontosAdversario, ref int energiaJogador, ref int energiaAdversario, Random rand)
    {
        string jogadorAtual = (currentPlayer == 1) ? jogador1 : jogador2;
        string jogadorAdversario = (currentPlayer == 1) ? jogador2 : jogador1;

        Console.WriteLine($"\nVez de {jogadorAtual}:");

        int[] cartas = new int[3];
        for (int i = 0; i < 3; i++)
        {
            cartas[i] = rand.Next(1, 7); // 1 to 6
            Console.WriteLine($"Carta {i + 1}: {ObterNomeCarta(cartas[i])}");
        }

        if (cartas.All(carta => carta == 1)) // Três Gols
        {
            Console.WriteLine($"Três Gols para {jogadorAtual}!");
            MarcarGol(ref golsJogador);
        }
        else if (cartas.All(carta => carta == 6)) // Três Energias
        {
            Console.WriteLine($"{jogadorAtual} ganhou mais uma energia!");
            energiaJogador++;
        }
        else if (cartas.All(carta => carta == 2)) // Três Pênaltis
        {
            ExecutarPenaltis(jogadorAtual, jogadorAdversario, ref golsJogador, ref golsAdversario, ref pontosJogador, ref pontosAdversario, rand);
        }
        else if (cartas.All(carta => carta == 3)) // Três Faltas
        {
            Console.WriteLine($"{jogadorAtual} cometeu três faltas e passa a vez para {jogadorAdversario}!");
        }
        else if (cartas.All(carta => carta == 4)) // Três Cartões Amarelos
        {
            Console.WriteLine($"{jogadorAtual} recebeu três cartões amarelos e perdeu duas energias!");
            energiaJogador -= 2;
            Advertir(jogadorAtual);
        }
        else if (cartas.All(carta => carta == 5)) // Três Cartões Vermelhos
        {
            Console.WriteLine($"{jogadorAtual} recebeu três cartões vermelhos e perdeu quatro energias!");
            energiaJogador -= 4;
        }
        else
        {
            int pontos = CalcularPontos(cartas);
            Console.WriteLine($"Pontuação para {jogadorAtual}: {pontos} pontos.");
            pontosJogador += pontos;
        }
    }

    static void ExecutarPenaltis(string jogadorAtual, string jogadorAdversario, ref int golsJogadorAtual, ref int golsJogadorAdversario,
        ref int pontosJogadorAtual, ref int pontosJogadorAdversario, Random rand)
    {
        Console.WriteLine($"{jogadorAtual} terá mais uma chance de pênalti!");

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"Escolha a direção do pênalti (Direita, Esquerda, Centro) para a tentativa {i + 1}:");
            string direcaoJogador = Console.ReadLine().ToLower();
            string direcaoAdversario = ObterDirecaoAleatoria(rand);

            if (direcaoJogador != direcaoAdversario)
            {
                Console.WriteLine($"{jogadorAtual} marcou um gol!");
                MarcarGol(ref golsJogadorAtual);
            }
            else
            {
                Console.WriteLine($"{jogadorAdversario} defendeu o pênalti de {jogadorAtual}!");
            }
        }
    }

    static void Advertir(string jogador)
    {
        Console.WriteLine($"Advertência: No próximo cartão amarelo, {jogador} perderá duas energias!");
    }

    static void MarcarGol(ref int gols)
    {
        gols++;
    }

    static int CalcularPontos(int[] cartas)
    {
        int pontos = 0;
        foreach (var carta in cartas)
        {
            switch (carta)
            {
                case 1: pontos += 3; break; // Gol
                case 2: pontos += 2; break; // Pênalti
                case 3: pontos += 1; break; // Falta
                case 4: pontos += 1; break; // Cartão Amarelo
                case 5: break; // Cartão Vermelho - nenhum ponto
                case 6: pontos += 2; break; // Energia
            }
        }
        return pontos;
    }

    static string ObterDirecaoAleatoria(Random rand)
    {
        string[] direcoes = { "direita", "esquerda", "centro" };
        return direcoes[rand.Next(0, 3)];
    }

    static string ObterNomeCarta(int carta)
    {
        switch (carta)
        {
            case 1: return "Gol";
            case 2: return "Pênalti";
            case 3: return "Falta";
            case 4: return "Cartão Amarelo";
            case 5: return "Cartão Vermelho";
            case 6: return "Energia";
            default: return "Desconhecido";
        }
    }

    static void MostrarResultadoFinal(string jogador1, string jogador2, int golsJogador1, int golsJogador2, int pontosJogador1, int pontosJogador2)
    {
        Console.WriteLine("\n--- RESULTADO FINAL ---");
        Console.WriteLine($"{jogador1}: Gols - {golsJogador1}, Pontos - {pontosJogador1}");
        Console.WriteLine($"{jogador2}: Gols - {golsJogador2}, Pontos - {pontosJogador2}");

        if (golsJogador1 > golsJogador2)
            Console.WriteLine($"Parabéns, {jogador1}! Você venceu com {golsJogador1} gols e {pontosJogador1} pontos.");
        else if (golsJogador2 > golsJogador1)
            Console.WriteLine($"Parabéns, {jogador2}! Você venceu com {golsJogador2} gols e {pontosJogador2} pontos.");
        else if (pontosJogador1 > pontosJogador2)
            Console.WriteLine($"Parabéns, {jogador1}! Você venceu com {golsJogador1} gols e {pontosJogador1} pontos.");
        else if (pontosJogador2 > pontosJogador1)
            Console.WriteLine($"Parabéns, {jogador2}! Você venceu com {golsJogador2} gols e {pontosJogador2} pontos.");
        else
            Console.WriteLine("O jogo empatou!");

        Console.WriteLine("------------------------");
    }
 }
