using CoopMedica.Database;
using CoopMedica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class BankMenu : AbstractMenu {
    protected override string Title => "Menu Banco";

    private readonly BankCollection bankCollection = new();

    protected override async Task Add() {
        Console.WriteLine("==== Adicionar Banco ====");
        string nome = Utils.ReadString("Nome: ");
        if (await bankCollection.Contains(x => x.Name == nome)) {
            Utils.Print("Já existe um banco com esse nome!", ConsoleColor.Red);
            return;
        }

        Bank bank = new() {
            Name = nome
        };

        await bankCollection.AddAsync(bank);
        Utils.Print("Dependente adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit() {
        Console.WriteLine("==== Editar Banco ====");
        Console.WriteLine("Digite o id do banco: ");
        int idBanco = Utils.ReadInt("> ", false) ?? default;
        if (!await bankCollection.Contains(x => x.Id == idBanco)) {
            Utils.Print("Não existe banco com este id!", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("Digite o novo nome do banco: ");
        string nome = Utils.ReadString("Nome: ");
        Bank bank = (await bankCollection.SelectOneAsync(x => x.Id == idBanco))!;
        bank.Name = nome;
        await bankCollection.UpdateAsync(bank);
        Utils.Print("Banco editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List() {
        Console.WriteLine("==== Listar Bancos ====");
        IEnumerable<Bank> banks = await bankCollection.SelectAsync();

        Table<Bank> banksTable = new();
        banksTable.RegisterColumn(name: "Id", function: x => x.Id.ToString())
            .RegisterColumn(name: "Nome", function: x => x.Name);

        banksTable.AddRows(banks);
        if (!banks.Any()) {
            Utils.Print("Não existem bancos cadastrados", ConsoleColor.Red);
            return;
        }
        banksTable.DisplayTable();
    }

    protected override async Task Remove() {
        Console.WriteLine("==== Remover Banco ====");
        Console.WriteLine("Digite o id do banco: ");
        int idCliente = Utils.ReadInt("> ", false) ?? default;
        int removed = await bankCollection.RemoveAsync(x => x.Id == idCliente);
        if (removed > 0) {
            Utils.Print("Banco removido com sucesso!", ConsoleColor.Green);
        } else {
            Utils.Print("Não existe um banco com este identificador!", ConsoleColor.Red);
        }
    }
}
