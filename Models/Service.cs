namespace CoopMedica.Models;

public class Service
{
    public int Id { get; set; }

    public string Name { get; set; }
    public float Cost { get; set; }
    public int MedicalSpecialtyId { get; set; }
    public int MedicId { get; set; }
    public int ClientId { get; set; }

}