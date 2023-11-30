using CoopMedica.Database;
using CoopMedica.Models;
using Org.BouncyCastle.Crypto.Prng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using CoopMedica.Views;

namespace CoopMedica.Menus;
public class ServiceMenu : AbstractMenu
{
    protected override string Title => "Menu Serviços";

    private ServiceCollection serviceCollection = new();
    private ServiceViewCollection serviceViewCollection = new();

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
        Table<ServiceView> serviceTable = new();
        IEnumerable<ServiceView> servicesData = await serviceViewCollection.SelectAsync();

        serviceTable.RegisterColumn("Id", x => x.ServiceId.ToString());
        serviceTable.RegisterColumn("Nome", x => x.ServiceName);
        serviceTable.RegisterColumn("Preço", x => x.ServiceCost.ToString("C2"));
        serviceTable.RegisterColumn("Especialidade", x => x.SpecialtyName);
        serviceTable.RegisterColumn("Médico", x => x.MedicName);
        serviceTable.RegisterColumn("Cliente", x => x.ClientName);



        if (!servicesData.Any()) {
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
