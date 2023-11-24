using CoopMedica.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Database;
public class PlanCollection : BaseCollection<Plan> {
    public override Plan ReadResult(MySqlDataReader rs) {
        Plan plan = new() {
            Id = rs.GetInt32("plan_id"),
            Name = rs.GetString("nome"),
            Discount= rs.GetFloat("desconto"),
            Price= rs.GetFloat("preco")
        };
        return plan;
    }

    protected override MySqlCommand GetDeleteSQL(Plan item) {
        MySqlCommand cmd = new("DELETE FROM plans WHERE plan_id = @plan");
        cmd.Parameters.AddWithValue("@plan", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(Plan item) {
        MySqlCommand cmd = new("INSERT INTO plans (nome, desconto, preco) VALUES (@name, @discount, @price)");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Parameters.AddWithValue("@discount", item.Discount);
        cmd.Parameters.AddWithValue("@price", item.Price);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM plans");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(Plan item) {
        MySqlCommand cmd = new("UPDATE plans SET nome = @name, desconto = @discount, preco = @price WHERE plan_id = @plan");
        cmd.Parameters.AddWithValue("@name", item.Name);
        cmd.Parameters.AddWithValue("@discount", item.Discount);
        cmd.Parameters.AddWithValue("@price", item.Price);
        cmd.Parameters.AddWithValue("@plan", item.Id);
        return cmd;
    }
}
