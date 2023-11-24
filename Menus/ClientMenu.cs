using CoopMedica.Models;
using CoopMedica.Database;
using CoopMedica.Services;
using MySql.Data.MySqlClient;

namespace CoopMedica.Menus;

public class ClientMenu
{
    public async static Task Run()
    {
        int opcao = CrudMenu.Run("Menu Cliente");
        PlanCollection planCollection = new();
        ClientCollection clientCollection = new();
        switch (opcao)
        {
            case 1:
                Console.WriteLine("==== Adicionar Cliente ====");
                Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
                string nome = Utils.ReadString("Nome: ");
                string cpf = Utils.ReadMaskedString("CPF: ", "   .   .   -  ");
                DateOnly dataNasc = Utils.ReadDate("Data de Nascimento: ");
                int idPlano = Utils.ReadInt("Id do Plano: ", false);
                if(!await planCollection.Contains(x => x.Id == idPlano)) {
                    Utils.Print("Não existe plano com este id!", ConsoleColor.Red);
                    break;
                }
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
                await clientCollection.AddAsync(novoCliente);
                Utils.Print("Cliente adicionado com sucesso!", ConsoleColor.Green);
                break;
            case 2:
                Console.WriteLine("==== Editar Cliente ====");
                Console.WriteLine("Digite o id do cliente: ");
                int idCliente = Utils.ReadInt("> ", false);
                if(!await clientCollection.Contains(x => x.Id == idCliente)) {
                    Utils.Print("Não existe cliente com este id!", ConsoleColor.Red);
                    break;
                }
                Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
                nome = Utils.ReadString("Nome: ");
                cpf = Utils.ReadMaskedString("CPF: ", "   .   .   -  ");
                dataNasc = Utils.ReadDate("Data de Nascimento: ");
                idPlano = Utils.ReadInt("Id do Plano: ", false);
                if(!await planCollection.Contains(x => x.Id == idPlano)) {
                    Utils.Print("Não existe plano com este id!", ConsoleColor.Red);
                    break;
                }
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
                await clientCollection.UpdateAsync(novoCliente);
                Utils.Print("Cliente editado com sucesso!", ConsoleColor.Green);        
                break;
            case 3:
                Console.WriteLine("==== Listar Clientes ====");
                IEnumerable<Client> clientes = await clientCollection.SelectAsync();
                foreach (Client cliente in clientes)
                {
                    Console.WriteLine(cliente);
                }
                if (!clientes.Any()) {
                    Utils.Print("Não existem clientes cadastrados", ConsoleColor.Red);
                }
                break;
            case 4:
                Console.WriteLine("==== Remover Cliente ====");
                Console.WriteLine("Digite o id do cliente: ");
                idCliente = Utils.ReadInt("> ", false);
                int removed = await clientCollection.RemoveAsync(x => x.Id == idCliente);
                if (removed > 0)
                    Utils.Print("Cliente removido com sucesso!", ConsoleColor.Green);
                else
                    Utils.Print("Não existe um cliente com este identificador", ConsoleColor.Red);
                break;
        }
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey(true);
    }
}