namespace CoopMedica.Models;

public class Dependant {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public required int ClientId { get; set; }
}