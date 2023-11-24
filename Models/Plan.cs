namespace CoopMedica.Models;

public class Plan {
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public float Discount {get;set;}
    public float Price {get;set;}

    public async Task<List<Service>> GetServices() {
        await Task.Delay(1);
        return new();
    }
}