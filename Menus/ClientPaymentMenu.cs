using CoopMedica.Database;
using CoopMedica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class ClientPaymentMenu : AbstractMenu {
    protected override string Title => "Menu Pagamento Cliente";

    private readonly ClientPaymentCollection clientPaymentCollection = new();

    private readonly ClientCollection clientCollection = new();

    private readonly BankCollection bankCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Pagamento Cliente ====");
        int idCliente = Utils.ReadInt("Id do Cliente: ") ?? default;
        if (!await clientCollection.Contains(x => x.Id == idCliente)) {
            Utils.Print("Não existe cliente com este id!", ConsoleColor.Red);
            return;
        }

        int idBanco = Utils.ReadInt("Id do Banco: ") ?? default;
        
        if (!await bankCollection.Contains(x => x.Id == idBanco)) {
            Utils.Print("Não existe banco com este id!", ConsoleColor.Red);
            return;
        }

        double valor = Utils.ReadDouble("Valor: ");

        ClientPayment clientPayment = new() {
            ClientId = idCliente,
            BankId = idBanco,
            Amount = (float)valor
        };

        await clientPaymentCollection.AddAsync(clientPayment);
        Utils.Print("Pagamento do Cliente adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Pagamento do Cliente ====");

        int idPagamento = Utils.ReadInt("> ", false) ?? default;
        if (!await clientPaymentCollection.Contains(x => x.Id == idPagamento)) {
            Utils.Print("Não existe pagamento com este id!", ConsoleColor.Red);
            return;
        }

        ClientPayment clientPayment = (await clientPaymentCollection.SelectOneAsync(x => x.Id == idPagamento))!;
        int idCliente = Utils.ReadInt("Id do Cliente: ", defaultValue: clientPayment.ClientId) ?? default;
        if (!await clientCollection.Contains(x => x.Id == idCliente)) {
            Utils.Print("Não existe cliente com este id!", ConsoleColor.Red);
            return;
        }
        clientPayment.ClientId = idCliente;

        int idBanco = Utils.ReadInt("Id do Banco: ", defaultValue: clientPayment.BankId) ?? default;
        if(!await bankCollection.Contains(x => x.Id == idBanco)) {
            Utils.Print("Não existe banco com este id!", ConsoleColor.Red);
            return;
        }
        clientPayment.BankId = idBanco;

        clientPayment.Amount = (float)Utils.ReadDouble("Valor: ", defaultValue: clientPayment.Amount);

        await clientPaymentCollection.UpdateAsync(clientPayment);
        Utils.Print("Dependente editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Pagamento dos Clientes ====");
        IEnumerable<ClientPayment> payments = await clientPaymentCollection.SelectAsync();

        
        Table<ClientPayment> dependantsTable = new();
        dependantsTable.RegisterColumn(name: "Id", function: x => x.Id.ToString())
            .RegisterColumn(name: "Id Cliente", function: x => x.ClientId.ToString())
            .RegisterColumn(name: "Id Banco", function: x => x.BankId.ToString())
            .RegisterColumn(name: "Valor", function: x => x.Amount.ToString("C2"));
        dependantsTable.AddRows(payments);

        if (!payments.Any()) {
            Utils.Print("Não existem pagamentos cadastrados", ConsoleColor.Red);
            return;
        }
        dependantsTable.DisplayTable();
    }

    protected override async Task Remove() {
        Console.WriteLine("==== Remover Pagamento do Cliente ====");
        Console.WriteLine("Digite o id do pagamento: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        int removed = await clientPaymentCollection.RemoveAsync(x => x.Id == idCliente);
        if (removed > 0) {
            Utils.Print("Pagamento removido com sucesso!", ConsoleColor.Green);
        } else {
            Utils.Print("Não existe um pagamento com este identificador!", ConsoleColor.Red);
        }
    }
}
