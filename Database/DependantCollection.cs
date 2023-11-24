using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class DependantCollection : BaseCollection<Dependant> {
    public override Dependant ReadResult(MySqlDataReader rs) {
        Dependant dpt = new() {
            Id = rs.GetInt32("dependant_id"),
            Nome = rs.GetString("nome"),
            ClientId = rs.GetInt32("client_id"),
        };
        return dpt;
    }

    protected override MySqlCommand GetDeleteSQL(Dependant item) {
        MySqlCommand cmd = new("DELETE FROM dependants WHERE dependant_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Dependant item) {
        MySqlCommand cmd = new("INSERT INTO dependants (nome, client_id) VALUES (@nome, @client_id)");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@client_id", item.ClientId);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM dependants");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Dependant item) {
        MySqlCommand cmd = new("UPDATE dependants SET nome = @nome, client_id = @client_id WHERE dependant_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@client_id", item.ClientId);
        return cmd;
    }
}
