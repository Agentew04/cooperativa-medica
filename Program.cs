
using CoopMedica.Services;

namespace CoopMedica;

public class Program {

    private static int CrudMenu() {
        Console.WriteLine("Escolha uma opção: ");
        Console.WriteLine("1 - Adicionar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Listar");
        Console.WriteLine("4 - Remover");
        int opcao = Utils.ReadInt("> ", false, new(1,4));
        return opcao;
    }


    private static void ClientMenu() {
        Console.WriteLine("==== Menu Cliente ====");
        int opcao = CrudMenu();
        switch(opcao) {
            case 1:
                break;
            default:
                Console.WriteLine("Input inválido");
                break;
        }
    }

    public async static Task Main(string[] args){
        await DatabaseService.Instance.SetupDatabase();

        Console.WriteLine("==== Menu Principal ====");
        Console.WriteLine("Escolha uma opção: ");
        Console.WriteLine("1 - Cliente");
        Console.WriteLine("2 - Dependente");
        Console.WriteLine("3 - Plano");
        Console.WriteLine("4 - Banco");
        Console.WriteLine("5 - Pagamento");
        Console.WriteLine("6 - Entidade Conveniada");
        Console.WriteLine("7 - Especialidade Médica");
        Console.WriteLine("8 - Serviço");
        
        int opcao = Utils.ReadInt("> ", false, new(1,8));

        switch(opcao) {
            case 1:
                ClientMenu();
                break;
            case 2:
                break;
            case 3:
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
            default:
                Utils.Print("Input inválido", ConsoleColor.Red);
                break;
        }
    }
}