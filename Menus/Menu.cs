using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopMedica.Menus; 
public class Menu {
    public static int DisplayMenu(string title, IEnumerable<string> choices) {
        Console.Clear(); // previne estouro do limite de linhas do console
        var origin = Console.GetCursorPosition();
        (int w, int h) = CalculateWindowSize(title, choices);
        DrawWindowFrame(origin, w, h);
        DrawTitle(origin, w,h, title);
        DrawOptions(origin, w, h, choices);
        int choice = GetUserChoice(origin, w,h, choices.Count());
        Console.SetCursorPosition(0, h + 2);
        return choice;
    }

    private static (int width, int height) CalculateWindowSize(string title, IEnumerable<string> choices) {
        int choiceMaxWidth = choices.Max(x => x.Length) + 4; //4 is 1pad + number + dot
        int titleWidth = title.Length + 2; //1 pad each side

        int maxWidth = Math.Max(choiceMaxWidth, titleWidth);

        int height = choices.Count() + 4;
        return (maxWidth, height);
    }

    private static void DrawWindowFrame((int Left, int Top) origin, int width, int height) {
        //Draw top
        Console.SetCursorPosition(origin.Left, origin.Top);
        Console.Write("╔");
        Console.Write(new string('═', width));
        Console.Write("╗");

        //Draw sides
        for (int i = 0; i < height; i++) {
            Console.SetCursorPosition(origin.Left, origin.Top + i + 1);
            Console.Write("║");
            Console.SetCursorPosition(origin.Left + width + 1, origin.Top + i + 1);
            Console.Write("║");
        }

        // Draw first separator on line 3
        Console.SetCursorPosition(origin.Left, origin.Top + 2);
        Console.Write("╠");
        Console.Write(new string('═', width));
        Console.Write("╣");

        // Draw second separator on line n-2
        Console.SetCursorPosition(origin.Left, origin.Top + height-1);
        Console.Write("╠");
        Console.Write(new string('═', width));
        Console.Write("╣");

        //Draw bottom
        Console.SetCursorPosition(origin.Left, origin.Top + height + 1);
        Console.Write("╚");
        Console.Write(new string('═', width));
        Console.Write("╝");
    }

    private static void DrawTitle((int Left, int Top) origin, int width, int height, string title) {
        // pad title with spaces
        int leftPad = (int)Math.Floor((width - (float)title.Length) / 2.0f);
        int rightPad = (int)Math.Ceiling((width - (float)title.Length) / 2.0f);

        string paddedTitle = new string(' ', leftPad) + title + new string(' ', rightPad);
        Console.SetCursorPosition(origin.Left+1, origin.Top+1);
        Console.Write(paddedTitle);
    }

    private static void DrawOptions((int Left, int Top) origin, int width, int height, IEnumerable<string> choices) {
        int i = 0;
        foreach (var choice in choices) {
            Console.SetCursorPosition(origin.Left + 2, origin.Top + 3 + i);
            Console.Write($"{i+1}.{choice}");
            i++;
        }
    }

    private static int GetUserChoice((int Left, int Top) origin, int width, int height, int choiceCount) {
        int inputMaxWidth = width-2;
        string input = "";
        bool error = false;
        while (true) {
            //limpa input
            Console.SetCursorPosition(origin.Left+2, origin.Top + height);
            Console.Write(new string(' ', inputMaxWidth));

            if (error) {
                Console.ForegroundColor = ConsoleColor.Red;
            } else {
                Console.ResetColor();
            }

            //escreve input
            Console.SetCursorPosition(origin.Left+2, origin.Top + height);
            Console.Write(input);

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter) {
                if (int.TryParse(input, out int choice)) {
                    if (choice >= 1 && choice <= choiceCount) {
                        return choice;
                    } else {
                        error = true;
                    }
                }
            } else if (key.Key == ConsoleKey.Backspace) {
                if (input.Length > 0) {
                    if (error) {
                        error = false;
                    }
                    input = input[..^1];
                }
            } else if (key.KeyChar >= '0' && key.KeyChar <= '9') {
                if (input.Length < inputMaxWidth) {
                    input += key.KeyChar;
                }
            }
        }
    }
}
