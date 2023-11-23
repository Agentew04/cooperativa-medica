namespace CoopMedica.Models;

public class Payment {
    public int Id { get; set; }

    public float Amount {get;set;}
    public bool IsPayed {get;set;}
    public Medic Medic {get;set;}
    public Client Client {get;set;}
}