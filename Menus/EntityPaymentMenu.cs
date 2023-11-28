using CoopMedica.Database;
using CoopMedica.Models;

namespace CoopMedica.Menus;
public class EntityPaymentMenu : AbstractMenu {
    protected override string Title => "Menu Carnê de Pagamento (Entidade)";

    private readonly EntityPaymentCollection entityPaymentCollection = new();

    private readonly AffiliatedEntityCollection affiliatedEntityCollection = new();

    private readonly BankCollection bankCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Carnê de Pagamento (Entidade) ====");
        int idCliente = Utils.ReadInt("Id da entidade afiliada: ") ?? default;
        if (!await affiliatedEntityCollection.Contains(x => x.Id == idCliente)) {
            Utils.Print("Não existe entidade afiliada com este id!", ConsoleColor.Red);
            return;
        }

        int idBanco = Utils.ReadInt("Id do Banco: ") ?? default;

        if (!await bankCollection.Contains(x => x.Id == idBanco)) {
            Utils.Print("Não existe banco com este id!", ConsoleColor.Red);
            return;
        }

        double valor = Utils.ReadDouble("Valor: ");

        EntityPayment clientPayment = new() {
            EntityId = idCliente,
            BankId = idBanco,
            Amount = (float)valor
        };

        await entityPaymentCollection.AddAsync(clientPayment);
        Utils.Print("Pagamento do Cliente adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Carnê de Pagamento (Entidade) ====");

        Console.WriteLine("Digite o id do pagamento: ");
        int idPagamento = Utils.ReadInt("> ", false) ?? default;
        if (!await entityPaymentCollection.Contains(x => x.Id == idPagamento)) {
            Utils.Print("Não existe pagamento com este id!", ConsoleColor.Red);
            return;
        }

        EntityPayment payment = (await entityPaymentCollection.SelectOneAsync(x => x.Id == idPagamento))!;
        int entityId = Utils.ReadInt("Id da Entidade: ", defaultValue: payment.EntityId) ?? default;
        if (!await affiliatedEntityCollection.Contains(x => x.Id == entityId)) {
            Utils.Print("Não existe entidade afiliada com este id!", ConsoleColor.Red);
            return;
        }
        payment.EntityId = entityId;

        int idBanco = Utils.ReadInt("Id do Banco: ", defaultValue: payment.BankId) ?? default;
        if (!await bankCollection.Contains(x => x.Id == idBanco)) {
            Utils.Print("Não existe banco com este id!", ConsoleColor.Red);
            return;
        }
        payment.BankId = idBanco;

        payment.Amount = (float)Utils.ReadDouble("Valor: ", defaultValue: payment.Amount);

        await entityPaymentCollection.UpdateAsync(payment);
        Utils.Print("Dependente editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Carnês de Pagamento (Entidades) ====");
        IEnumerable<EntityPayment> payments = await entityPaymentCollection.SelectAsync();


        Table<EntityPayment> paymentsTable = new();
        paymentsTable.RegisterColumn(name: "Id", function: x => x.Id.ToString())
            .RegisterColumn(name: "Id Entidade", function: x => x.EntityId.ToString())
            .RegisterColumn(name: "Id Banco", function: x => x.BankId.ToString())
            .RegisterColumn(name: "Valor", function: x => x.Amount.ToString("C2"));
        paymentsTable.AddRows(payments);

        if (!payments.Any()) {
            Utils.Print("Não existem carnês de pagamento cadastrados", ConsoleColor.Red);
            return;
        }
        paymentsTable.DisplayTable();
    }

    protected override async Task Remove() {
        Console.WriteLine("==== Remover Carnê de Pagamento (Entidade) ====");
        Console.WriteLine("Digite o id do pagamento: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        int removed = await entityPaymentCollection.RemoveAsync(x => x.Id == idCliente);
        if (removed > 0) {
            Utils.Print("Pagamento removido com sucesso!", ConsoleColor.Green);
        } else {
            Utils.Print("Não existe um pagamento com este identificador!", ConsoleColor.Red);
        }
    }
}
