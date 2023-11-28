using CoopMedica.Models;
using CoopMedica.Database;
using CoopMedica.Services;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoopMedica.Menus;

public class ClientMenu : AbstractMenu
{
    protected override string Title => "Menu Cliente";

    private readonly ClientCollection clientCollection = new();
    private readonly PlanCollection planCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Cliente ====");
        Console.WriteLine("Digite o nome, cpf, data de nascimento do cliente e id plano: ");
        string nome = Utils.ReadString("Nome: ");
        string cpf = Utils.ReadMaskedString("CPF: ", "   .   .   -  ");
        DateOnly dataNasc = Utils.ReadDate("Data de Nascimento: ");
        int? idPlano = Utils.ReadInt("Id do Plano: ", allowEmpty: true);
        if (idPlano is not null && !await planCollection.Contains(x => x.Id == idPlano)) {
            Utils.Print("Não existe plano com este id!", ConsoleColor.Red);
            return;
        }
        Client novoCliente = new() {
            Nome = nome,
            Cpf = cpf,
            DataNascimento = dataNasc,
            PlanId = idPlano
        };
        await clientCollection.AddAsync(novoCliente);
        Utils.Print("Cliente adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Cliente ====");
        Console.WriteLine("Digite o id do cliente: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        if (!await clientCollection.Contains(x => x.Id == idCliente)) {
            Utils.Print("Não existe cliente com este id!", ConsoleColor.Red);
            return;
        }
        Client client = (await clientCollection.SelectOneAsync(x => x.Id == idCliente))!;
        client.Nome = Utils.ReadString("Nome: ", defaultValue: client.Nome);
        client.Cpf = Utils.ReadMaskedString("CPF: ", "   .   .   -  ", client.Cpf);
        client.DataNascimento = Utils.ReadDate("Data de Nascimento: ", defaultValue: client.DataNascimento);
        client.PlanId = Utils.ReadInt("Id do Plano: ", defaultValue: client.PlanId, allowEmpty: true);
        if (client.PlanId is not null && !await planCollection.Contains(x => x.Id == client.PlanId)) {
            Utils.Print("Não existe plano com este id!", ConsoleColor.Red);
            return;
        }
        await clientCollection.UpdateAsync(client);
        Utils.Print("Cliente editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Clientes ====");
        IEnumerable<Client> clientes = await clientCollection.SelectAsync();
        Table<Client> clientesTable = new();
        clientesTable.RegisterColumn("Id", function: x => x.Id.ToString())
            .RegisterColumn(name: "Nome", function: x => x.Nome)
            .RegisterColumn(name: "CPF", function: x => x.Cpf)
            .RegisterColumn(name: "Data de Nascimento", function: x => x.DataNascimento.ToString("dd/MM/yyyy"))
            .RegisterColumn(name: "Plano", function: x => x.PlanId.ToString() ?? "Sem plano");
        clientesTable.AddRows(clientes);
        if (!clientes.Any()) {
            Utils.Print("Não existem clientes cadastrados", ConsoleColor.Red);
            return;
        }
        clientesTable.DisplayTable();
    }

    protected override async Task Remove() {
        Console.WriteLine("==== Remover Cliente ====");
        Console.WriteLine("Digite o id do cliente: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        int removed = await clientCollection.RemoveAsync(x => x.Id == idCliente);
        if (removed > 0) {
            Utils.Print("Cliente removido com sucesso!", ConsoleColor.Green);
        } else {
            Utils.Print("Não existe um cliente com este identificador", ConsoleColor.Red);
        }
    }
}