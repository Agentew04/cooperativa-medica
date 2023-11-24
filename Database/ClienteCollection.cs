using CoopMedica.Models;
using MySql.Data.MySqlClient;

namespace CoopMedica.Database; 

public class ClienteCollection : BaseCollection<Client> {
    public override Client ReadResult(MySqlDataReader rs) {
        Client c = new Client {
            Id = rs.GetInt32(0),
            Nome = rs.GetString(1),
            Cpf = rs.GetString(2),
            DataNascimento = DateOnly.FromDateTime(rs.GetDateTime(3))
        };
        if (!rs.IsDBNull(4)) {
            c.Plan = null;
        }
        else {
            PlanCollection pc = new();
            // TODO get plan
        }
        return c;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT (client_id, nome, cpf, data_nasc, plan_id) FROM clients");
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Client item) {
        MySqlCommand cmd = new("INSERT INTO clients (nome, cpf, data_nasc, plan_id) VALUES (@nome, @cpf, @data_nasc, @plan_id)");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@cpf", item.Cpf);
        cmd.Parameters.AddWithValue("@data_nasc", item.DataNascimento);
        cmd.Parameters.AddWithValue("@plan_id", item.Plan?.Id);
        return cmd;
    }

    protected override MySqlCommand GetDeleteSQL(Client item) {
        MySqlCommand cmd = new("DELETE FROM clients WHERE client_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Client item) {
        MySqlCommand cmd = new("UPDATE clients SET nome = @nome, cpf = @cpf, data_nasc = @data_nasc, plan_id = @plan_id WHERE client_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@cpf", item.Cpf);
        cmd.Parameters.AddWithValue("@data_nasc", item.DataNascimento);
        cmd.Parameters.AddWithValue("@plan_id", item.Plan?.Id);
        return cmd;
    }
}