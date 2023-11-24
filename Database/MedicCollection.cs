using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class MedicCollection : BaseCollection<Medic> {
    public override Medic ReadResult(MySqlDataReader rs) {
        Medic medic = new() {
            Id = rs.GetInt32("medic_id"),
            Nome = rs.GetString("nome"),
            SpecialtyId = rs.GetInt32("speciality_id")
        };
        return medic;
    }

    protected override MySqlCommand GetDeleteSQL(Medic item) {
        MySqlCommand cmd = new("DELETE FROM medic WHERE medic_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Medic item) {
        MySqlCommand cmd = new("INSERT INTO medic (nome, speciality_id) VALUES (@nome, @speciality_id)");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@speciality_id", item.SpecialtyId);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM medic");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Medic item) {
        MySqlCommand cmd = new("UPDATE medic SET nome = @nome, speciality_id = @speciality_id WHERE medic_id = @id");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@speciality_id", item.SpecialtyId);
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }
}
