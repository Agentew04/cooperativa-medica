using CoopMedica.Models;
using CoopMedica.Database;
using CoopMedica.Services;

namespace CoopMedica.Menus;

public class PlanMenu
{
    public async static Task Run()
    {
        int opcao = CrudMenu.Run("Menu Plano");
        PlanCollection planoCollection = new();
        switch (opcao)
        {
            case 1:
                Console.WriteLine("==== Adicionar Plano ====");
                Console.WriteLine("Digite o nome, valor e desconto: ");
                string nome = Utils.ReadString("Nome: ");
                double valor = Utils.ReadDouble("Valor: ");
                double desconto = Utils.ReadDouble("Desconto: ");
                Plan novoPlano = new()
                {
                    Name = nome,
                    Price = (float)valor,
                    Discount = (float)desconto

                };
                await planoCollection.AddAsync(novoPlano);
                Utils.Print("Plano adicionado com sucesso!", ConsoleColor.Green);
                break;
            case 2:
                Console.WriteLine("==== Editar Plano ====");
                Console.WriteLine("Digite o id do plano: ");
                int idPlano = Utils.ReadInt("> ", false);
                if(!await planoCollection.Contains(x => x.Id == idPlano)) {
                    Utils.Print("Não existe um plano com este id!", ConsoleColor.Red);
                    break;
                }

                Console.WriteLine("Digite os novos nome, valor e desconto do plano: ");
                nome = Utils.ReadString("> ");
                valor = Utils.ReadDouble("> ");
                desconto = Utils.ReadDouble("> ");
                novoPlano = new()
                {
                    Id = idPlano,
                    Name = nome,
                    Price = (float)valor,
                    Discount = (float)desconto
                };
                await planoCollection.UpdateAsync(novoPlano);
                Console.WriteLine("Plano editado com sucesso!");
                break;
            case 3:
                Console.WriteLine("==== Listar Planos ====");
                IEnumerable<Plan> planos = await planoCollection.SelectAsync();
                foreach (Plan plano in planos)
                {
                    Console.WriteLine(plano);
                }
                if (!planos.Any()) {
                    Utils.Print("Não existe nenhum plano cadastrado", ConsoleColor.Red);
                }
                break;
            case 4:
                Console.WriteLine("==== Remover Plano ====");
                Console.WriteLine("Digite o id do plano: ");
                idPlano = Utils.ReadInt("> ", false);

                // check availability
                if(!await planoCollection.Contains(x => x.Id == idPlano)) {
                    Utils.Print("Não existe nenhum plano com este id!", ConsoleColor.Red);
                }

                int removed = await planoCollection.RemoveAsync(x => x.Id == idPlano);
                if(removed > 0)
                    Utils.Print("Plano removido com sucesso!", ConsoleColor.Green);
                break;
            default:
                Utils.Print("Input inválido", ConsoleColor.Red);
                break;
        }
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey(true); // espera por uma tecla
    }
}