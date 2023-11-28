using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class DependantMenu : AbstractMenu {
    protected override string Title => "Menu Dependente";

    private DependantCollection dependantCollection = new();
    private ClientCollection clientCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Dependente ====");
        Console.WriteLine("Digite o id do cliente: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        if (!await clientCollection.Contains(x => x.Id == idCliente)) {
            Utils.Print("Não existe cliente com este id!", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("Digite o nome do dependente: ");
        string nome = Utils.ReadString("Nome: ");
        Dependant dependant = new() {
            Nome = nome,
            ClientId = idCliente
        };
        await dependantCollection.AddAsync(dependant);
        Utils.Print("Dependente adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Dependente ====");
        Console.WriteLine("Digite o id do dependente: ");
        int idDependende = Utils.ReadInt("> ", false) ?? default;
        if (!await dependantCollection.Contains(x => x.Id == idDependende)) {
            Utils.Print("Não existe dependente com este id!", ConsoleColor.Red);
            return;
        }
        
        Console.WriteLine("Digite o novo nome do dependente: ");
        string nome = Utils.ReadString("Nome: ");
        Dependant dep = (await dependantCollection.SelectOneAsync(x => x.Id == idDependende))!;
        dep.Nome = nome;
        await dependantCollection.UpdateAsync(dep);
        Utils.Print("Dependente editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Dependentes ====");
        IEnumerable<Dependant> clientes = await dependantCollection.SelectAsync();

        var clientDeps =
            (await dependantCollection.SelectAsync())
            .Join(
                await clientCollection.SelectAsync(),
                x => x.ClientId, x => x.Id, (dep, client) => (dep, client)
            );

        Table<(Dependant dep, Client client)> dependantsTable = new();
        dependantsTable.RegisterColumn(name: "Id", function: x => x.dep.Id.ToString())
            .RegisterColumn(name: "Nome Dependente", function: x => x.dep.Nome)
            .RegisterColumn(name: "Id Cliente", function: x => x.client.Id.ToString())
            .RegisterColumn(name: "Nome Cliente", function: x => x.client.Nome);
        dependantsTable.AddRows(clientDeps);
        if (!clientes.Any()) {
            Utils.Print("Não existem dependentes cadastrados", ConsoleColor.Red);
            return;
        }
        dependantsTable.DisplayTable();
    }
    protected override async Task Remove() {
        Console.WriteLine("==== Remover Dependente ====");
        Console.WriteLine("Digite o id do dependente: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        int removed = await dependantCollection.RemoveAsync(x => x.Id == idCliente);
        if (removed > 0) {
            Utils.Print("Dependente removido com sucesso!", ConsoleColor.Green);
        } else {
            Utils.Print("Não existe um dependente com este identificador!", ConsoleColor.Red);
        }
    }
}
