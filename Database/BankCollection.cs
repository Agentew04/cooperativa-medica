using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class BankCollection : BaseCollection<Bank> {
    public override Bank ReadResult(MySqlDataReader rs) {
        Bank bank = new() {
            Id = rs.GetInt32("bank_id"),
            Name = rs.GetString("name")
        };
        return bank;
    }

    protected override MySqlCommand GetDeleteSQL(Bank item) {
        MySqlCommand cmd = new("DELETE FROM banks WHERE bank_id = @bank");
        cmd.Parameters.AddWithValue("@bank", item.Id);
        cmd.Prepare();
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Bank item) {
        MySqlCommand cmd = new("INSERT INTO banks (name) VALUES (@name)");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Prepare();
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM banks");
        cmd.Prepare();
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Bank item) {
        MySqlCommand cmd = new("UPDATE banks SET name = @name WHERE bank_id = @bank");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Parameters.AddWithValue("@bank", item.Id);
        cmd.Prepare();
        return cmd;
    }
}
