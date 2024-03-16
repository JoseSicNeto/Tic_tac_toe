using System;

public class Program
{
    public static void Main(string[] args)
    {
        // Inicialize o jogo
        Jogo jogo = new Jogo();
        jogo.Jogar();
    }
}

public class Jogo
{
     private bool modoUnicoJogador = false;
     private string jogador_atual = "X";
     private string[] locais = new string[9]
     {"1", "2", "3", "4", "5", "6", "7", "8", "9"};
     private string[] locais_disponiveis = new string[9]
     {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

     public Jogo()
     {
          //Construtor Padrão.
     }

     public void Jogar()
     {
          //Deixando o Random Fora dos Loops para não o Chamar Desnecessariamente.
          Random random = new Random();

          while (true)
          {
               //Chama o Método do Modo antes do Jogo Começar.
               AlternarModo();

               //Loop Infinito até o Jogo Acabar.
               while (true)
               {
                    OuputGame();

                    //Outro Loop até o Jogador fazer sua Jogada (Certa).
                    while (true)
                    {
                         Console.Write($"\nJogador {jogador_atual}, sua vez. Diga-me sua jogada: ");
                         string jogada = Console.ReadLine();
                         Console.WriteLine();
                         
                         if (VerificarJogada(jogada))
                         {
                              InserirJogada(int.Parse(jogada) - 1);
                              break;
                         }
                         else
                              Console.WriteLine("Local indisponivel / Local Inexistente");
                    }

                    //Termina o Jogo, Caso Algum Deles seja Verdadeiro. 
                    if (FimDoJogo())
                         break;
                    
                    //Apenas Executa se o Modo for Unico (UM JOGADOR).
                    if (modoUnicoJogador)
                    {
                         //Loop com Jogadas Aleatórias até acertar uma Válida.
                         AlternarJogador();
                        
                         OuputGame();
                         Console.WriteLine("\n-------------------\n");
                             
                         if (VerificaJogadaDecisiva())  
                         {  
                              // se Existir, Executa ela.  
                         }
                         else
                         {
                              // Faz a Jogada Aleatoria, se não for Decisiva.
                              while (true)
                              {
                                   int jogada = random.Next(1, 10);
                              
                                   if (VerificarJogada(jogada.ToString()))
                                   {
                                        InserirJogada(jogada - 1);
                                        break;
                                   }
                              }    
                         }
                    }

                    if (FimDoJogo())
                         break;

                    //Altera o Jogador no Final não Importando o Modo.               
                    AlternarJogador();
               }

               if (!JogarNovamente())
                    break;
          }

          Console.WriteLine("\n\nObrigado Por Jogar!\n");
     }

     private void OuputGame()
     {
          //Mostra o Tabuleiro / Jogo. 
          Console.WriteLine(
                              "|  " + locais[0] + "  |  " + locais[1] + "  |  " + locais[2] + "  |\n" +
                              "|  " + locais[3] + "  |  " + locais[4] + "  |  " + locais[5] + "  |\n" +
                              "|  " + locais[6] + "  |  " + locais[7] + "  |  " + locais[8] + "  |"
                           );

     }
     private void AlternarModo()
     {
          Console.WriteLine("-------------------------------\n" +
                            "|  1. Jogador vs Jogador      |\n" +
                            "|  2. Jogador vs Computador   |\n" +
                            "-------------------------------"
                           );

          //Decide o Modo do Jogo --- Em Progresso...
          while (true)
          {
               Console.Write("Digite um dos Modos: ");
               string escolha = Console.ReadLine();

               if (escolha == "1" || escolha == "2")
               {
                    if (escolha == "2")
                         modoUnicoJogador = true;
                    break;
               }
          }
     }

     private bool VerificarPosicoes(int local1, int local2, int local3)
     {
          return locais[local1] == locais[local2] && locais[local1] == locais[local3] && (locais[local1] == "X" || locais[local1] == "O");
     }

     private bool VerificarVencedor()
     {
          // Verifica as Linhas.
          for (int i = 0; i < 9; i += 3)
               if (VerificarPosicoes(i, i + 1, i + 2))
                    return true;

          // Verifica as Colunas.
          for (int i = 0; i < 3; i++)
               if (VerificarPosicoes(i, i + 3, i + 6))
                    return true;

          // Verifica as Diagonais.
          if (VerificarPosicoes(0, 4, 8) || VerificarPosicoes(2, 4, 6))
               return true;

          return false;
     }

     private void FraseDoVencedor()
     {
          // Apenas Imprime qual Jogador Venceu.
          if (VerificarVencedor())
               Console.WriteLine($"Jogador {jogador_atual} Ganhou");
     }

     private bool VerificaJogadaDecisiva()
     {
          // Verifica as Linhas.
          for (int i = 0; i < 9; i += 3)
               if(ChecaSequencia(i, i + 3, 1))
                    return true;

          // Verifica as Colunas.
          for (int i = 0; i < 3; i ++)
               if(ChecaSequencia(i, i + 7, 3))
                    return true;

          // Verifica a Diagonal Principal.
          if(ChecaSequencia(0, 9, 4))
               return true;

          // Verifica a Diagonal Secundária.
          if(ChecaSequencia(2, 7, 2))
               return true;
          
          return false;
     }
    
     private bool ChecaSequencia(int inicio, int fim, int passo)
     {
          // Checa a Sequencia tanto no Ataque quanto na Defesa.
          return VerificaSequencia("O", inicio, fim, passo) || VerificaSequencia("X", inicio, fim, passo);
     }
    
     private bool VerificaSequencia(string jogada ,int inicio, int fim, int passo)
     {
          // Verifica se o Jogador ou o Bot Vencerá na Próxima Jogada.
          for (int i = inicio; i < fim; i += passo)
          {
               int count = 0;
               int index = -1;
               for(int k = i; k < fim; k += passo)
               {
                    if (locais[k] == jogada)
                         count++;
                    else if (locais[k] != "O" && locais[k] != "X")
                         index = k;
               }
        
               if (count == 2 && index != -1)
               {
                    RealizaJogadaBot(index);
                    return true;
               }
          }
    
          return false;
     }

     private void RealizaJogadaBot(int jogada)
     {
          // Insere a Jogador do Bot no Jogo.
          locais[jogada] = "O";
          locais_disponiveis[jogada] = "0";
     }

     private bool VerificarJogada(string jogada)
     {
          bool jogadaValida = false;

          // Verifica a Jogada, se ela, Existe, é Possível.
          for (int i = 0; i < 9; i++)
               if (jogada == locais_disponiveis[i])
                    jogadaValida = true;

          return jogadaValida;
     }

     private void InserirJogada(int jogada)
     {
          //Insere a Jogada no Jogo.
          locais_disponiveis[jogada] = "0";
          locais[jogada] = jogador_atual;
     }

     public bool VerificarEmpate()
     {
          bool empate = true;

          // Verifica se já foram Todas Jogadas.
          for (int i = 0; i < 9; i++)
               if (locais_disponiveis[i] != "0")                         
                    empate = false;

          if (empate)
               Console.WriteLine("\nNenhum Vencedor. Empate");
          
          return empate;
     }
     
     private void AlternarJogador()
     {
          //Troca X por O ou O por X.
          jogador_atual = jogador_atual == "X" ? "O" : "X";
     }

     private bool JogarNovamente()
     {
          // Verifica se o(os) Jogador(es) Quer(em) Jogar Novamente. 
          bool resposta = false;
          string inputResposta;
          Console.WriteLine("\nDeseja Jogar Novamente?");

          while (true)
          {
               Console.Write("Resposta: ");
               inputResposta = Console.ReadLine().ToLower();

               if (inputResposta == "sim" || inputResposta == "nao")
                    break;
               else
                    Console.WriteLine("Por Favor, responda com 'sim' ou 'nao'.");
          }

          // Reseta todos os atributos para o Padrão.
          if (inputResposta == "sim")
          {
               jogador_atual = "X";
               locais = new string[9] {"1", "2", "3", "4", "5", "6", "7", "8", "9"};
               locais_disponiveis = new string[9] {"1", "2", "3", "4", "5", "6", "7", "8", "9"};
               resposta = true;
          }
          else if (inputResposta == "nao")
               resposta = false;

          return resposta;
     }

     private bool FimDoJogo()
     {
          // Verifica se o Jogo Atual já Terminou.
          if (VerificarVencedor() || VerificarEmpate())
          {
               FraseDoVencedor();
               OuputGame();
               return true;
          }
          
          return false;
     }
}