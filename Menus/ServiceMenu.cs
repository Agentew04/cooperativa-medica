using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus;
public class ServiceMenu : AbstractMenu
{
    protected override string Title => "Menu Serviços";

    private ServiceCollection serviceCollection = new();

    protected override async Task Add()
    {
        Console.WriteLine("==== Adicionar Serviço ====");

        Console.WriteLine("Digite o nome do serviço: ");
        string nome = Utils.ReadString("Nome: ");
        float preco = (float)Utils.ReadDouble("Preço: ");
        int idEspecialidade = Utils.ReadInt("Id da especialidade médica: ") ?? default;
        int idMedico = Utils.ReadInt("Id do médico: ") ?? default;
        int idCliente = Utils.ReadInt("Id do cliente: ") ?? default;
        Service service = new()
        {
            Name = nome,
            Cost = preco,
            MedicId = idMedico,
            ClientId = idCliente,
            MedicalSpecialtyId = idEspecialidade
        };
        await serviceCollection.AddAsync(service);
        Utils.Print("Serviço adicionado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task Edit()
    {
        Console.WriteLine("==== Editar Serviço ====");
        Console.WriteLine("Digite o id do serviço: ");
        int idServico = Utils.ReadInt("> ", false) ?? default;
        if (!await serviceCollection.Contains(x => x.Id == idServico))
        {
            Utils.Print("Não existe serviço com este id!", ConsoleColor.Red);
            return;
        }

        Service service = (await serviceCollection.SelectOneAsync(x => x.Id == idServico))!;
        Console.WriteLine("Digite o novo nome do serviço: ");
        service.Name = Utils.ReadString("Nome: ", defaultValue: service.Name);
        service.Cost = (float)Utils.ReadDouble("Preço: ", defaultValue: service.Cost);
        service.MedicalSpecialtyId = Utils.ReadInt("Id da especialidade médica: ", defaultValue: service.MedicalSpecialtyId) ?? default;
        service.MedicId = Utils.ReadInt("Id do médico: ", defaultValue: service.MedicId) ?? default;
        service.ClientId = Utils.ReadInt("Id do cliente: ", defaultValue: service.ClientId) ?? default;
        await serviceCollection.UpdateAsync(service);
        Utils.Print("Serviço editado com sucesso!", ConsoleColor.Green);
    }

    protected override async Task List()
    {
        Console.WriteLine("==== Listar Serviços ====");
        IEnumerable<Service> services = await serviceCollection.SelectAsync();
        Table<(Service service, MedicalSpecialty specialty, Medic med, Client cli)> serviceTable = new();
        var servicesData = services.Join(
            await new MedicalSpecialtyCollection().SelectAsync(),
            x => x.MedicalSpecialtyId, x => x.Id, (service, specialty) => (service, specialty)
        ).Join(
            await new MedicCollection().SelectAsync(),
            x => x.service.MedicId, x => x.Id, (service, med) => (service.service, service.specialty, med)
        ).Join(
            await new ClientCollection().SelectAsync(),
            x => x.service.ClientId, x => x.Id, (service, cli) => (service.service, service.specialty, service.med, cli)
        ).Select(x => (x.service, x.specialty, x.med, x.cli));
        serviceTable.RegisterColumn(name: "Id", function: x => x.service.Id.ToString())
            .RegisterColumn(name: "Nome", function: x => x.service.Name)
            .RegisterColumn(name: "Preço", function: x => x.service.Cost.ToString("C"))
            .RegisterColumn(name: "Id Especialidade", function: x => x.specialty.Id.ToString())
            .RegisterColumn(name: "Nome Especialidade", function: x => x.specialty.Nome)
            .RegisterColumn(name: "Id Médico", function: x => x.med.Id.ToString())
            .RegisterColumn(name: "Nome Médico", function: x => x.med.Nome)
            .RegisterColumn(name: "Id Cliente", function: x => x.cli.Id.ToString())
            .RegisterColumn(name: "Nome Cliente", function: x => x.cli.Nome);

        if (servicesData.Count() == 0)
        {
            Utils.Print("Não existem serviços cadastrados!", ConsoleColor.Red);
            return;
        }

        serviceTable.AddRows(servicesData);

        serviceTable.DisplayTable();
    }
    protected override async Task Remove()
    {
        Console.WriteLine("==== Remover Serviço ====");
        Console.WriteLine("Digite o id do serviço: ");
        int idServico = Utils.ReadInt("> ", false) ?? default;
        int removed = await serviceCollection.RemoveAsync(x => x.Id == idServico);
        if (removed > 0)
        {
            Utils.Print("Serviço removido com sucesso!", ConsoleColor.Green);
        }
        else
        {
            Utils.Print("Não existe um serviço com este identificador!", ConsoleColor.Red);
        }
    }
}
