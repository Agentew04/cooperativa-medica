using CoopMedica.Models;
using CoopMedica.Database;
using CoopMedica.Services;

namespace CoopMedica.Menus;

public class ClientMenu
{
    public async static Task Run()
    {
        Console.WriteLine("==== Menu Cliente ====");
        int opcao = CrudMenu.Run();
        switch (opcao)
        {
            case 1:
                Console.WriteLine("==== Adicionar Cliente ====");
                Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
                string nome = Utils.ReadString(">");
                string cpf = Utils.ReadString(">");
                DateOnly dataNasc = Utils.ReadDate(">");
                int idPlano = Utils.ReadInt("", false);
                Client novoCliente = new()
                {
                    Nome = nome,
                    Cpf = cpf,
                    DataNascimento = dataNasc,
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
                nome = Utils.ReadString(">");
                cpf = Utils.ReadString(">");
                dataNasc = Utils.ReadDate(">");
                idPlano = Utils.ReadInt("", false);
                novoCliente = new()
                {
                    Id = idCliente,
                    Nome = nome,
                    Cpf = cpf,
                    DataNascimento = dataNasc,
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
}