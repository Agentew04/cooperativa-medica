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
            Name = rs.GetString("name"),
            Price = rs.GetDecimal("price"),
            Coverage = rs.GetDecimal("coverage"),
            Bank = new() {
                Id = rs.GetInt32("bank_id"),
                Name = rs.GetString("bank_name")
            }
        };
        return plan;
    }

    protected override MySqlCommand GetDeleteSQL(Plan item) {
        throw new NotImplementedException();
    }

    protected override MySqlCommand GetInsertSQL(Plan item) {
        throw new NotImplementedException();
    }

    protected override MySqlCommand GetSelectSQL() {
        throw new NotImplementedException();
    }

    protected override MySqlCommand GetUpdateSQL(Plan item) {
        throw new NotImplementedException();
    }
}
