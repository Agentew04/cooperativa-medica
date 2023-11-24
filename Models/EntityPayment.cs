using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Models; 

/// <summary>
/// Classe que representa um carnê de pagamento do banco
/// para uma entidade afiliada.
/// 
/// Uma entidade afiliada pode ser um hospital, uma clínica ou
/// apenas um médico que atua sozinho em um consultório.
/// </summary>
public class EntityPayment {

    /// <summary>
    /// O id unico deste pagamento.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// O id da entidade afiliada que recebeu este pagamento.
    /// </summary>
    public int EntityId { get; set; }

    /// <summary>
    /// O id do banco que fez este pagamento.
    /// </summary>
    public int BankId { get; set; }
    
    /// <summary>
    /// A quantia deste pagamento
    /// </summary>
    public float Amount { get; set; }
}
