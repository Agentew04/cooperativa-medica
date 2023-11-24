namespace CoopMedica.Models;

public class AffiliatedEntity {
    public int Id { get; set; }

    public required string Nome { get; set; }

    public required string Cnpj { get; set; }

    public async Task<List<Medic>> GetMedics() {
        // TODO get medics from database
        await Task.Delay(100);
        return new();
    }
}