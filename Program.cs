
using CoopMedica.Services;
using CoopMedica.Database;
using CoopMedica.Models;

namespace CoopMedica;

public class Program
{

    private static int CrudMenu()
    {
        Console.WriteLine("Escolha uma opção: ");
        Console.WriteLine("1 - Adicionar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Listar");
        Console.WriteLine("4 - Remover");
        int opcao = Utils.ReadInt("> ", false, new(1, 4));
        return opcao;
    }


    private async static Task ClientMenu()
    {
        Console.WriteLine("==== Menu Cliente ====");
        int opcao = CrudMenu();
        switch (opcao)
        {
            case 1:
                Console.WriteLine("==== Adicionar Cliente ====");
                Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
                string nome = Console.ReadLine() ?? "";
                string cpf = Console.ReadLine() ?? "";
                DateTime dataNasc = DateTime.Parse(Console.ReadLine() ?? "1990/01/01");
                int idPlano = Utils.ReadInt("", false);
                Client novoCliente = new()
                {
                    Nome = nome,
                    Cpf = cpf,
                    DataNascimento = DateOnly.FromDateTime(dataNasc),
                    Plan = new Plan()
                    {
                        Id = idPlano
                    }
                };
                ClientCollection clienteCollection = new();
                await clienteCollection.AddAsync(novoCliente);
                Console.WriteLine("Cliente adicionado com sucesso!");
                break;
            case 2:
                Console.WriteLine("==== Editar Cliente ====");
                Console.WriteLine("Digite o id do cliente: ");
                int idCliente = Utils.ReadInt("", false);
                Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
                nome = Console.ReadLine() ?? "";
                cpf = Console.ReadLine() ?? "";
                dataNasc = DateTime.Parse(Console.ReadLine() ?? "1990/01/01");
                idPlano = Utils.ReadInt("", false);
                novoCliente = new()
                {
                    Id = idCliente,
                    Nome = nome,
                    Cpf = cpf,
                    DataNascimento = DateOnly.FromDateTime(dataNasc),
                    Plan = new Plan()
                    {
                        Id = idPlano
                    }
                };
                clienteCollection = new();
                await clienteCollection.Update(novoCliente);
                Console.WriteLine("Cliente editado com sucesso!");
                break;
            case 3:
                Console.WriteLine("==== Listar Clientes ====");
                clienteCollection = new();
                IEnumerable<Client> clientes = await clienteCollection.SelectAsync();
                foreach (Client cliente in clientes)
                {
                    Console.WriteLine(cliente);
                }
                break;
            default:
                Console.WriteLine("Input inválido");
                break;
        }
    }

    public static async Task Main(string[] args)
    {
        await DatabaseService.Instance.SetupDatabase();
        while (true)
        {
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

            int opcao = Utils.ReadInt("> ", false, new(1, 8));

            switch (opcao)
            {
                case 1:
                    await ClientMenu();
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
}