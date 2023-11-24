namespace CoopMedica.Menus;

public class CrudMenu
{
    public static int Run()
    {
        Console.WriteLine("Escolha uma opção: ");
        Console.WriteLine("1 - Adicionar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Listar");
        Console.WriteLine("4 - Remover");
        int opcao = Utils.ReadInt("> ", false, new(1, 4));
        return opcao;
    }
}