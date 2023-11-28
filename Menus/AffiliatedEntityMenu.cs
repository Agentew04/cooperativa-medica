using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class AffiliatedEntityMenu : AbstractMenu
{
    protected override string Title => "Menu Entidade Afiliada";

    private AffiliatedEntityCollection affiliatedEntityCollection = new();

    protected override async Task Add()
    {
        Console.WriteLine("==== Adicionar Entidade Afiliada ====");

        Console.WriteLine("Digite o nome da entidade afiliada: ");
        string nome = Utils.ReadString("Nome: ");
        string cnpj = Utils.ReadString("CNPJ: ");
        AffiliatedEntity affiliatedEntity = new()
        {
            Nome = nome,
            Cnpj = cnpj
        };
        await affiliatedEntityCollection.AddAsync(affiliatedEntity);
        Utils.Print("Entidade afiliada adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit()
    {
        Console.WriteLine("==== Editar Entidade Afiliada ====");
        Console.WriteLine("Digite o id da entidade afiliada: ");
        int idEntidadeAfiliada = Utils.ReadInt("> ", false) ?? default;
        if (!await affiliatedEntityCollection.Contains(x => x.Id == idEntidadeAfiliada))
        {
            Utils.Print("Não existe entidade afiliada com este id!", ConsoleColor.Red);
            return;
        }

        AffiliatedEntity aff = (await affiliatedEntityCollection.SelectOneAsync(x => x.Id == idEntidadeAfiliada))!;
        aff.Nome = Utils.ReadString("Nome: ", defaultValue: aff.Nome);
        aff.Cnpj = Utils.ReadString("CNPJ: ", defaultValue: aff.Cnpj);
        await affiliatedEntityCollection.UpdateAsync(aff);
        Utils.Print("Entidade afiliada editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List()
    {
        Console.WriteLine("==== Listar Entidade Afiliada ====");
        IEnumerable<AffiliatedEntity> affiliatedEntities = await affiliatedEntityCollection.SelectAsync();
        Table<AffiliatedEntity> table = new();
        table.RegisterColumn("Id", x => x.Id.ToString());
        table.RegisterColumn("Nome", x => x.Nome);
        table.RegisterColumn("Cpnj", x => x.Cnpj);
        table.AddRows(affiliatedEntities);
        if (!affiliatedEntities.Any())
        {
            Utils.Print("Não existe nenhuma entidade afiliada cadastrada", ConsoleColor.Red);
            return;
        }
        table.DisplayTable();
    }
    protected override async Task Remove()
    {
        Console.WriteLine("==== Remover Entidade Afiliada ====");
        Console.WriteLine("Digite o id da entidade afiliada: ");
        int idAffiliatedEntity = Utils.ReadInt("> ", false);
        int removed = await affiliatedEntityCollection.RemoveAsync(x => x.Id == idAffiliatedEntity);
        if (removed > 0)
        {
            Utils.Print("Entidade afiliada removida com sucesso!", ConsoleColor.Green);
        }
        else
        {
            Utils.Print("Não existe uma entidade afiliada com este identificador!", ConsoleColor.Red);
        }
    }
}
