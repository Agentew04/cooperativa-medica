namespace CoopMedica.Models;

public class Medic {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public int SpecialtyId { get; set; }

    public int AffiliatedEntityId { get; set; }
}