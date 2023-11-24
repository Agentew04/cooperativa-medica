
using CoopMedica.Services;
using CoopMedica.Menus;

namespace CoopMedica;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.ResetColor();
        Console.Clear();
        await DatabaseService.Instance.SetupDatabase();
        bool rodando = true;
        while (rodando)
        {
            int opcao = Menu.DisplayMenu("Menu Principal", new List<string> {
                "Cliente",
                "Dependente",
                "Plano",
                "Banco",
                "Pagamento",
                "Entidade Conveniada",
                "Especialidade Médica",
                "Serviço",
                "Sair"
            });

            switch (opcao)
            {
                case 1:
                    await ClientMenu.Run();
                    break;
                case 2:
                    break;
                case 3:
                    await PlanMenu.Run();
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    rodando = false;
                    break;
                default:
                    Utils.Print("Input inválido", ConsoleColor.Red);
                    break;
            }
        }
    }
}