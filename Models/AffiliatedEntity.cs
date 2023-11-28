namespace CoopMedica.Models;

/// <summary>
/// Classe que representa uma entidade afiliada ao sistema.
/// Normalmente vai ser um hospital ou uma clinica.
/// 
/// Tamb�m pode ser um <i>wrapper</i> para um m�dico que atua
/// sozinho em um consult�rio. Neste caso existira um objeto
/// <see cref="Medic"/> que tem um link atrav�s de <see cref="Medic."/>
/// </summary>
public class AffiliatedEntity
{
    public int Id { get; set; }

    public required string Nome { get; set; }

    public required string Cnpj { get; set; }
}