namespace CoopMedica.Models;

/// <summary>
/// Classe que representa uma entidade afiliada ao sistema.
/// Normalmente vai ser um hospital ou uma clinica.
/// 
/// Também pode ser um <i>wrapper</i> para um médico que atua
/// sozinho em um consultório. Neste caso existira um objeto
/// <see cref="Medic"/> que tem um link através de <see cref="Medic."/>
/// </summary>
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