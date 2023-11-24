namespace CoopMedica.Models;

/// <summary>
/// Classe que representa um pagamento do cliente para o banco.
/// </summary>
public class ClientPayment {
    /// <summary>
    /// O id unico deste pagamento.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// O id do cliente que fez este pagamento.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// O id do banco que recebeu este pagamento.
    /// </summary>
    public int BankId { get; set; }
    
    /// <summary>
    /// A quantia paga.
    /// </summary>
    public float Amount { get; set; }
}