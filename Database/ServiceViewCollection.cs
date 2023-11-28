using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopMedica.Views;

namespace CoopMedica.Database;
public class ServiceViewCollection : BaseCollection<ServiceView>
{
    public override ServiceView ReadResult(MySqlDataReader rs)
    {
        return new ServiceView
        {
            ServiceId = rs.GetInt32("ServiceId"),
            ServiceName = rs.GetString("ServiceName"),
            ServiceCost = rs.GetFloat("ServiceCost"),
            SpecialtyId = rs.GetInt32("SpecialtyId"),
            SpecialtyName = rs.GetString("SpecialtyName"),
            MedicId = rs.GetInt32("MedicId"),
            MedicName = rs.GetString("MedicName"),
            ClientId = rs.GetInt32("ClientId"),
            ClientName = rs.GetString("ClientName"),
        };
    }

    protected override MySqlCommand GetDeleteSQL(ServiceView item)
    {
        MySqlCommand cmd = new("DELETE FROM services WHERE service_id = @service");
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(ServiceView item)
    {
        MySqlCommand cmd = new("INSERT INTO services (nome, preco, speciality_id, client_id, medic_id) VALUES (@name, @price, @speciality, @client, @medic)");
        cmd.Parameters.AddWithValue("@client", item.ClientId);
        cmd.Parameters.AddWithValue("@medic", item.MedicId);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL()
    {
        MySqlCommand cmd = new("SELECT * FROM ServiceView");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(ServiceView item)
    {
        MySqlCommand cmd = new("UPDATE services SET nome = @name, preco = @price, speciality_id = @speciality, client_id = @client, medic_id = @medic WHERE service_id = @id");
        cmd.Parameters.AddWithValue("@client", item.ClientId);
        cmd.Parameters.AddWithValue("@medic", item.MedicId);
        return cmd;
    }
}
