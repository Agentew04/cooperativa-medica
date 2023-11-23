namespace CoopMedica.Models;

public class Medic {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public MedicalSpecialty Specialty {get;set;}
    public AffiliatedEntity Entity {get;set;}
}