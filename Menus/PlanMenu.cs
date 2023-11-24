using CoopMedica.Models;
using CoopMedica.Database;
using CoopMedica.Services;

namespace CoopMedica.Menus;

public class PlanMenu
{
    public async static Task Run()
    {
        Console.WriteLine("==== Menu Plano ====");
        int opcao = CrudMenu.Run();
        switch (opcao)
        {
            case 1:
                Console.WriteLine("==== Adicionar Plano ====");
                Console.WriteLine("Digite o nome, valor e desconto: ");
                string nome = Utils.ReadString(">");
                double valor = Utils.ReadDouble(">");
                double desconto = Utils.ReadDouble(">", false);
                Plan novoPlano = new()
                {
                    Name = nome,
                    Price = (float)valor,
                    Discount = (float)desconto

                };
                PlanCollection planoCollection = new();
                await planoCollection.AddAsync(novoPlano);
                Console.WriteLine("Plano adicionado com sucesso!");
                break;
            case 2:
                Console.WriteLine("==== Editar Plano ====");
                Console.WriteLine("Digite o id do plano: ");
                int idPlano = Utils.ReadInt("", false);
                Console.WriteLine("Digite o nome, valor e id entidade conveniada: ");
                nome = Utils.ReadString(">");
                valor = Utils.ReadDouble(">");
                desconto = Utils.ReadDouble("", false);
                novoPlano = new()
                {
                    Id = idPlano,
                    Name = nome,
                    Price = (float)valor,
                    Discount = (float)desconto
                };
                planoCollection = new();
                await planoCollection.Update(novoPlano);
                Console.WriteLine("Plano editado com sucesso!");
                break;
            case 3:
                Console.WriteLine("==== Listar Planos ====");
                planoCollection = new();
                IEnumerable<Plan> planos = await planoCollection.SelectAsync();
                foreach (Plan plano in planos)
                {
                    Console.WriteLine(plano);
                }
                break;
            case 4:
                Console.WriteLine("==== Remover Plano ====");
                Console.WriteLine("Digite o id do plano: ");
                idPlano = Utils.ReadInt("", false);
                planoCollection = new();
                await planoCollection.RemoveAsync(x => x.Id == idPlano);
                Console.WriteLine("Plano removido com sucesso!");
                break;
            default:
                Utils.Print("Input inválido", ConsoleColor.Red);
                break;
        }
    }
}