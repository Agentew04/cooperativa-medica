namespace CoopMedica.Menus;

public class CrudMenu
{
    public static int Run(string title)
    {
        int opcao = Menu.DisplayMenu(title, new List<string> {
            "Adicionar",
            "Editar",
            "Listar",
            "Remover"
        });
        return opcao;
    }
}