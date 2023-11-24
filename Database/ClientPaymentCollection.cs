using CoopMedica.Models;
using MySql.Data.MySqlClient;

namespace CoopMedica.Database;
public class ClientPaymentCollection : BaseCollection<ClientPayment> {
    public override ClientPayment ReadResult(MySqlDataReader rs) {
        ClientPayment payment = new() {
            Id = rs.GetInt32("client_payment_id"),
            ClientId = rs.GetInt32("client_id"),
            BankId = rs.GetInt32("bank_id"),
            Amount = rs.GetFloat("amount")
        };
        return payment;
    }

    protected override MySqlCommand GetDeleteSQL(ClientPayment item) {
        MySqlCommand cmd = new("DELETE FROM client_payments WHERE payment_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        return cmd;
    }

    protected override MySqlCommand GetInsertSQL(ClientPayment item) {
        MySqlCommand cmd = new("INSERT INTO client_payments (client_id, bank_id, valor) VALUES (@client_id, @bank_id, @amount)");
        cmd.Parameters.AddWithValue("@client_id", item.ClientId);
        cmd.Parameters.AddWithValue("@bank_id", item.BankId);
        cmd.Parameters.AddWithValue("@amount", item.Amount);
        return cmd;
    }

    protected override MySqlCommand GetSelectSQL() {
        MySqlCommand cmd = new("SELECT * FROM client_payments");
        return cmd;
    }

    protected override MySqlCommand GetUpdateSQL(ClientPayment item) {
        MySqlCommand cmd = new("UPDATE client_payments SET client_id = @client_id, bank_id = @bank_id, valor = @amount WHERE client_payment_id = @id");
        cmd.Parameters.AddWithValue("@id", item.Id);
        cmd.Parameters.AddWithValue("@client_id", item.ClientId);
        cmd.Parameters.AddWithValue("@bank_id", item.BankId);
        cmd.Parameters.AddWithValue("@amount", item.Amount);
        return cmd;
    }
}
