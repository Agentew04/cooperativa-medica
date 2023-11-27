namespace CoopMedica.Models;

public class Bank {
    public int Id { get; set; }

    public required string Name {get;set;}

    public List<ClientPayment> GetClientPayments() {
        // todo implement
        return new();
    }

    public List<EntityPayment> GetEntityPayments() {
        // TODO implement
        // talvez esses dois metodos n sejam aqui
        // e na vdd implemente no BankCollection
        // com views???
        return new();
    }
}