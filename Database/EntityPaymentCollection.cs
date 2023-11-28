using CoopMedica.Models;
using MySql.Data.MySqlClient;

namespace CoopMedica.Database;
public class EntityPaymentCollection : BaseCollection<EntityPayment>
{
    public override EntityPayment ReadResult(MySqlDataReader rs)
    {
        EntityPayment entityPayment = new()
        {
            Id = rs.GetInt32("entity_payment_id"),
            EntityId = rs.GetInt32("affiliated_entity_id"),
            BankId = rs.GetInt32("bank_id"),
            Amount = rs.GetFloat("valor")
        };
        return entityPayment;
    }

    protected override MySqlCommand GetDeleteSQL(EntityPayment item)
    {
        MySqlCommand cmd = new("DELETE FROM entity_payments WHERE entity_payment_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(EntityPayment item)
    {
        MySqlCommand cmd = new("INSERT INTO entity_payments (affiliated_entity_id, bank_id, valor) VALUES (@affiliated_entity_id, @bank_id, @amount)");
        cmd.Parameters.AddWithValue("@affiliated_entity_id", item.EntityId);
        cmd.Parameters.AddWithValue("@bank_id", item.BankId);
        cmd.Parameters.AddWithValue("@amount", item.Amount);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL()
    {
        MySqlCommand cmd = new("SELECT * FROM entity_payments");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(EntityPayment item)
    {
        MySqlCommand cmd = new("UPDATE entity_payments SET affiliated_entity_id = @affiliated_entity_id, bank_id = @bank_id, valor = @amount WHERE entity_payment_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        cmd.Parameters.AddWithValue("@affiliated_entity_id", item.EntityId);
        cmd.Parameters.AddWithValue("@bank_id", item.BankId);
        cmd.Parameters.AddWithValue("@amount", item.Amount);
        return cmd;
    }
}
