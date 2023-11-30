using CoopMedica.Models;
using CoopMedica.Database;

namespace CoopMedica.Menus;

public class PlanMenu : AbstractMenu
{
    protected override string Title => "Menu Plano";

    private readonly PlanCollection planCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Plano ====");
        Console.WriteLine("Digite o nome, valor e desconto: ");
        string nome = Utils.ReadString("Nome: ");
        double valor = Utils.ReadDouble("Valor: ");
        double desconto = Utils.ReadDouble("Desconto: ");
        Plan novoPlano = new() {
            Name = nome,
            Price = (float)valor,
            Discount = (float)desconto

        };
        await planCollection.AddAsync(novoPlano);
        Utils.Print("Plano adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Plano ====");
        Console.WriteLine("Digite o id do plano: ");
        int idPlano = Utils.ReadInt("> ", false) ?? default;
        if (!await planCollection.Contains(x => x.Id == idPlano)) {
            Utils.Print("Não existe um plano com este id!", ConsoleColor.Red);
            return;
        }

        Plan plan = (await planCollection.SelectOneAsync(x => x.Id == idPlano))!;

        plan.Name = Utils.ReadString("Nome: ", defaultValue: plan.Name);
        plan.Price = (float)Utils.ReadDouble("Valor: ", defaultValue: plan.Price);
        plan.Discount =(float) Utils.ReadDouble("Desconto: ", defaultValue: plan.Discount);
        
        await planCollection.UpdateAsync(plan);
        Console.WriteLine("Plano editado com sucesso!");
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Planos ====");
        IEnumerable<Plan> planos = await planCollection.SelectAsync();

        Table<Plan> planosTable = new();
        planosTable.RegisterColumns(
            new Table<Plan>.Column("Id", x => x.Id.ToString()),
            new Table<Plan>.Column("Nome", x => x.Name),
            new Table<Plan>.Column("Valor", x => x.Price.ToString("C")),
            new Table<Plan>.Column("Desconto", x => x.Discount.ToString("P")));
        planosTable.AddRows(planos);

        if (!planos.Any()) {
            Utils.Print("Não existe nenhum plano cadastrado", ConsoleColor.Red);
            return;
        }

        planosTable.DisplayTable();
    }

    protected override async Task Remove() {
        Console.WriteLine("==== Remover Plano ====");
        Console.WriteLine("Digite o id do plano: ");
        int idPlano = Utils.ReadInt("> ", false) ?? default;

        // check availability
        if (!await planCollection.Contains(x => x.Id == idPlano)) {
            Utils.Print("Não existe nenhum plano com este id!", ConsoleColor.Red);
            return;
        }

        int removed = await planCollection.RemoveAsync(x => x.Id == idPlano);
        if (removed > 0) {
            Utils.Print("Plano removido com sucesso!", ConsoleColor.Green);
        }
    }
}