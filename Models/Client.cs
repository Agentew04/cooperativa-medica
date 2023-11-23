namespace CoopMedica.Models;

public class Client {
    public int Id {get;set;}

    public string Nome {get;set;} = "";
    public string CPF {get;set;} = "";
    public DateOnly DataNascimento {get;set;} = DateOnly.MinValue;

    public List<Dependant> Dependants {get;set;} = new();
    public Plan Plan {get;set;} = new();
}