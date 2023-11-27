﻿using CoopMedica.Services;
using CoopMedica.Menus;
using CoopMedica.Models;

namespace CoopMedica;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.ResetColor();
        Console.Clear();
        await DatabaseService.Instance.SetupDatabase();
        bool rodando = true;
        while (rodando)
        {
            int opcao = Menu.DisplayMenu("Menu Principal", new List<string> {
                "Cliente",
                "Dependente",
                "Plano",
                "Banco",
                "Pagamento",
                "Entidade Conveniada",
                "Especialidade Médica",
                "Serviço",
                "Sair"
            });

            switch (opcao)
            {
                case 1:
                    ClientMenu clientMenu = new();
                    await clientMenu.Run();
                    break;
                case 2:
                    DependantMenu dependantMenu = new();
                    await dependantMenu.Run();
                    break;
                case 3:
                    PlanMenu planMenu = new();
                    await planMenu.Run();
                    break;
                case 4:
                    BankMenu bankMenu = new();
                    await bankMenu.Run();
                    break;
                case 5:
                    ClientPaymentMenu clientPaymentMenu = new();
                    await clientPaymentMenu.Run();
                    break;
                case 6:

                    break;
                case 7:
                    MedicalSpecialtyMenu medicalSpecialtyMenu = new();
                    await medicalSpecialtyMenu.Run();
                    break;
                case 8:
                    break;
                case 9:
                    rodando = false;
                    break;
                default:
                    Utils.Print("Input inválido", ConsoleColor.Red);
                    break;
            }
        }
    }
}