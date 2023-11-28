using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class MedicalSpecialtyMenu : AbstractMenu
{
    protected override string Title => "Menu Especialidade Médica";

    private MedicalSpecialtyCollection medicalSpecialtyCollection = new();

    protected override async Task Add()
    {
        Console.WriteLine("==== Adicionar Especialidade Médica ====");

        Console.WriteLine("Digite o nome da especialidade médica: ");
        string nome = Utils.ReadString("Nome: ");
        MedicalSpecialty medicalSpecialty = new()
        {
            Nome = nome,
        };
        await medicalSpecialtyCollection.AddAsync(medicalSpecialty);
        Utils.Print("Especialidade médica adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit()
    {
        Console.WriteLine("==== Editar Especialidade Médica ====");
        Console.WriteLine("Digite o id da especialidade médica: ");
        int idEspecialidade = Utils.ReadInt("> ", false) ?? default;
        if (!await medicalSpecialtyCollection.Contains(x => x.Id == idEspecialidade))
        {
            Utils.Print("Não existe especialidade médica com este id!", ConsoleColor.Red);
            return;
        }

        MedicalSpecialty med = (await medicalSpecialtyCollection.SelectOneAsync(x => x.Id == idEspecialidade))!;
        Console.WriteLine("Digite o novo nome da especialidade médica: ");
        med.Nome = Utils.ReadString("Nome: ", defaultValue: med.Nome);
        await medicalSpecialtyCollection.UpdateAsync(med);
        Utils.Print("Especialidade médica editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List()
    {
        Console.WriteLine("==== Listar Especialidades Médicas ====");
        IEnumerable<MedicalSpecialty> medicalSpecialties = await medicalSpecialtyCollection.SelectAsync();
        Table<MedicalSpecialty> table = new();
        table.RegisterColumn("Id", x => x.Id.ToString());
        table.RegisterColumn("Nome", x => x.Nome);
        table.AddRows(medicalSpecialties);
        if (!medicalSpecialties.Any())
        {
            Utils.Print("Não existe nenhuma especialidade cadastrada", ConsoleColor.Red);
            return;
        }
        table.DisplayTable();
    }
    protected override async Task Remove()
    {
        Console.WriteLine("==== Remover Especialidade Médica ====");
        Console.WriteLine("Digite o id da especialidade médica: ");
        int idEspecialidade = Utils.ReadInt("> ", false) ?? default;
        int removed = await medicalSpecialtyCollection.RemoveAsync(x => x.Id == idEspecialidade);
        if (removed > 0)
        {
            Utils.Print("Especialidade médica removida com sucesso!", ConsoleColor.Green);
        }
        else
        {
            Utils.Print("Não existe uma especialidade médica com este identificador!", ConsoleColor.Red);
        }
    }
}
