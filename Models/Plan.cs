namespace CoopMedica.Models;

public class Plan {
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public List<Service> Services {get;set;} = new();
    public float Discount {get;set;}
    public float Price {get;set;}
}