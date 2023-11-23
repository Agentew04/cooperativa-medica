namespace CoopMedica.Models;

public class MedicalSpecialty {
    public int Id { get; set; }

    public required string Nome { get; set; }
    public List<Service> Services {get;set;} = new();
}