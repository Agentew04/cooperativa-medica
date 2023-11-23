namespace CoopMedica.Models;

public class Dependant {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public Client client {get;set;}
}