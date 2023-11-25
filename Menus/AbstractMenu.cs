using CoopMedica.Database;
using CoopMedica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus; 
public abstract class AbstractMenu {
    private const int ADD = 1;
    private const int EDIT = 2;
    private const int LIST = 3;
    private const int REMOVE = 4;

    protected abstract string Title { get; }

    public async Task Run() {
        int opcao = CrudMenu.Run(Title);
        switch (opcao) {
            case ADD:
                await Add();
                break;
            case EDIT:
                await Edit();
                break;
            case LIST:
                await List();
                break;
            case REMOVE:
                await Remove();
                break;
        }
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey(true);
    }

    protected abstract Task Add();

    protected abstract Task Edit();

    protected abstract Task List();

    protected abstract Task Remove();
}
