using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class MedicCollection : BaseCollection<Medic>
{
    public override Medic ReadResult(MySqlDataReader rs)
    {
        Medic medic = new()
        {
            Id = rs.GetInt32("medic_id"),
            Nome = rs.GetString("nome"),
            SpecialtyId = rs.GetInt32("speciality_id"),
            AffiliatedEntityId = rs.GetInt32("affiliated_entity_id")
        };
        return medic;
    }

    protected override MySqlCommand GetDeleteSQL(Medic item)
    {
        MySqlCommand cmd = new("DELETE FROM medics WHERE medic_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Medic item)
    {
        MySqlCommand cmd = new("INSERT INTO medics (nome, speciality_id, affiliated_entity_id) VALUES (@nome, @speciality_id, @affiliated_entity_id)");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@speciality_id", item.SpecialtyId);
        cmd.Parameters.AddWithValue("@affiliated_entity_id", item.AffiliatedEntityId);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL()
    {
        MySqlCommand cmd = new("SELECT * FROM medics");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Medic item)
    {
        MySqlCommand cmd = new("UPDATE medics SET nome = @nome, speciality_id = @speciality_id WHERE medic_id = @id");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@speciality_id", item.SpecialtyId);
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }
}
