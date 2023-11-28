using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class MedicMenu : AbstractMenu
{
    protected override string Title => "Menu Médico";

    private MedicCollection medicCollection = new();
    private MedicalSpecialtyCollection medicalSpecialtyCollection = new();
    private AffiliatedEntityCollection affiliatedEntityCollection = new();

    protected override async Task Add()
    {
        Console.WriteLine("==== Adicionar Médico ====");

        Console.WriteLine("Digite o nome do médico: ");
        string nome = Utils.ReadString("Nome: ");
        int idEspecialidade = Utils.ReadInt("Id da especialidade: ");
        int idEntidade = Utils.ReadInt("Id da entidade: ");
        Medic medic = new()
        {
            Nome = nome,
            AffiliatedEntityId = idEntidade,
            SpecialtyId = idEspecialidade
        };
        await medicCollection.AddAsync(medic);
        Utils.Print("Médico adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit()
    {
        Console.WriteLine("==== Editar Médico ====");
        Console.WriteLine("Digite o id do médico: ");
        int idMedico = Utils.ReadInt("> ", false);
        if (!await medicCollection.Contains(x => x.Id == idMedico))
        {
            Utils.Print("Não existe médico com este id!", ConsoleColor.Red);
            return;
        }

        Console.WriteLine("Digite o novo nome do médico: ");
        string nome = Utils.ReadString("Nome: ");
        int idEspecialidade = Utils.ReadInt("Id da especialidade: ");
        int idEntidade = Utils.ReadInt("Id da entidade: ");
        Medic med = (await medicCollection.SelectOneAsync(x => x.Id == idMedico))!;
        med.Nome = nome;
        med.AffiliatedEntityId = idEntidade;
        med.SpecialtyId = idEspecialidade;
        await medicCollection.UpdateAsync(med);
        Utils.Print("Médico editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List()
    {
        Console.WriteLine("==== Listar Médicos ====");
        IEnumerable<Medic> medics = await medicCollection.SelectAsync();
        // liste os atributos dos médicos e faça um join com as especialidades e entidades
        Table<(Medic med, AffiliatedEntity aff, MedicalSpecialty spec)> medicsTable = new();
        var medicsData = medics.Join(
            await affiliatedEntityCollection.SelectAsync(),
            x => x.AffiliatedEntityId, x => x.Id, (med, aff) => (med, aff)
        ).Join(
            await medicalSpecialtyCollection.SelectAsync(),
            x => x.med.SpecialtyId, x => x.Id, (med, spec) => (med.med, med.aff, spec)
        );
        medicsTable.RegisterColumn(name: "Id", function: x => x.med.Id.ToString())
            .RegisterColumn(name: "Nome", function: x => x.med.Nome)
            .RegisterColumn(name: "Id Entidade", function: x => x.aff.Id.ToString())
            .RegisterColumn(name: "Nome Entidade", function: x => x.aff.Nome)
            .RegisterColumn(name: "Id Especialidade", function: x => x.spec.Id.ToString())
            .RegisterColumn(name: "Nome Especialidade", function: x => x.spec.Nome);
        medicsTable.AddRows(medicsData);
        if (!medics.Any())
        {
            Utils.Print("Não existem médicos cadastrados", ConsoleColor.Red);
            return;
        }
        medicsTable.DisplayTable();
    }
    protected override async Task Remove()
    {
        Console.WriteLine("==== Remover Médico ====");
        Console.WriteLine("Digite o id do médico: ");
        int idEspecialidade = Utils.ReadInt("> ", false);
        int removed = await medicCollection.RemoveAsync(x => x.Id == idEspecialidade);
        if (removed > 0)
        {
            Utils.Print("Médico removido com sucesso!", ConsoleColor.Green);
        }
        else
        {
            Utils.Print("Não existe um médico com este identificador!", ConsoleColor.Red);
        }
    }
}
