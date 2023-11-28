using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class ServiceCollection : BaseCollection<Service>
{
    public override Service ReadResult(MySqlDataReader rs)
    {
        Service service = new()
        {
            Id = rs.GetInt32("service_id"),
            Name = rs.GetString("nome"),
            Cost = rs.GetFloat("preco"),
            MedicalSpecialtyId = rs.GetInt32("speciality_id"),
            MedicId = rs.GetInt32("medic_id"),
            ClientId = rs.GetInt32("client_id")
        };
        return service;
    }

    protected override MySqlCommand GetDeleteSQL(Service item)
    {
        MySqlCommand cmd = new("DELETE FROM services WHERE service_id = @service");
        cmd.Parameters.AddWithValue("@service", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Service item)
    {
        MySqlCommand cmd = new("INSERT INTO services (nome, preco, speciality_id, client_id, medic_id) VALUES (@name, @price, @speciality, @client, @medic)");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Parameters.AddWithValue("@price", item.Cost);
        cmd.Parameters.AddWithValue("@speciality", item.MedicalSpecialtyId);
        cmd.Parameters.AddWithValue("@client", item.ClientId);
        cmd.Parameters.AddWithValue("@medic", item.MedicId);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL()
    {
        MySqlCommand cmd = new("SELECT * FROM services");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Service item)
    {
        MySqlCommand cmd = new("UPDATE services SET nome = @name, preco = @price, speciality_id = @speciality, client_id = @client, medic_id = @medic WHERE service_id = @id");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Parameters.AddWithValue("@price", item.Cost);
        cmd.Parameters.AddWithValue("@speciality", item.MedicalSpecialtyId);
        cmd.Parameters.AddWithValue("@client", item.ClientId);
        cmd.Parameters.AddWithValue("@medic", item.MedicId);
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }
}
