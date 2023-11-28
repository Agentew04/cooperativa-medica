namespace CoopMedica.Models;

/// <summary>
/// Classe do modelo que representa um cliente da cooperativa.
/// </summary>
public class Client {

    /// <summary>
    /// O id principal do cliente.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// O nome do cliente
    /// </summary>
    public string Nome { get; set; } = "";

    /// <summary>
    /// O cpf do cliente
    /// </summary>
    public string Cpf { get; set; } = "";

    /// <summary>
    /// Data de nascimento
    /// </summary>
    public DateOnly DataNascimento { get; set; } = DateOnly.MinValue;

    /// <summary>
    /// O plano que o cliente contrata. É nulo se o cliente
    /// não contratou nenhum plano ou se o plano foi cancelado.
    /// </summary>
    public int? PlanId { get; set; }

    /// <summary>
    /// Pega do banco de dados uma lista com os dependentes
    /// </summary>
    /// <returns>A lista com os dependentes</returns>
    public async Task<List<Dependant>> GetDependantsAsync() {
        // TODO implement this
        await Task.Delay(500);
        return new List<Dependant>();
    }

    public override string ToString() {
        return $"{Id} {Nome} {Cpf} {DataNascimento} {PlanId}";
    }
}