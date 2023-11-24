namespace CoopMedica.Models;

public class Bank {
    public int Id { get; set; }

    public string Name {get;set;}

    public List<ClientPayment> Payments {get;set;} = new();
}