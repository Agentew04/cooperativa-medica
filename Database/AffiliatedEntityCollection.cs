using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class AffiliatedEntityCollection : BaseCollection<AffiliatedEntity> {
    public override AffiliatedEntity ReadResult(MySqlDataReader rs) {
        AffiliatedEntity affiliatedEntity = new() {
            Id = rs.GetInt32("affiliated_entity_id"),
            Nome = rs.GetString("nome"),
            Cnpj = rs.GetString("cnpj")
        };
        return affiliatedEntity;
    }

    protected override MySqlCommand GetDeleteSQL(AffiliatedEntity item) {
        MySqlCommand cmd = new("DELETE FROM affiliated_entities WHERE affiliated_entity_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(AffiliatedEntity item) {
        MySqlCommand cmd = new("INSERT INTO affiliated_entities (nome, cnpj) VALUES (@nome, @cnpj)");
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@cnpj", item.Cnpj);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM affiliated_entities");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(AffiliatedEntity item) {
        MySqlCommand cmd = new("UPDATE affiliated_entities SET nome = @nome, cnpj = @cnpj WHERE affiliated_entity_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        cmd.Parameters.AddWithValue("@nome", item.Nome);
        cmd.Parameters.AddWithValue("@cnpj", item.Cnpj);
        return cmd;
    }
}
