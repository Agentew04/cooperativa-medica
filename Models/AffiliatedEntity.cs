namespace CoopMedica.Models;

public class AffiliatedEntity {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public List<Medic> medics {get;set;}
}